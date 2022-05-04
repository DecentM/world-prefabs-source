﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DecentM.Subtitles.Vtt
{
    public class Parser
    {
        public enum NodeKind
        {
            Unknown,
            VttAst,
            Header,
            Note,
            TimestampStart,
            TimestampArrow,
            TimestampEnd,
            TextContents,
            Parameters,
        }

        public struct Node
        {
            public Node(NodeKind kind, string value)
            {
                this.kind = kind;
                this.value = value;
            }

            public Node(NodeKind kind, int value)
            {
                this.kind = kind;
                this.value = value;
            }

            public Node(NodeKind kind, Dictionary<string, string> value)
            {
                this.kind = kind;
                this.value = value;
            }

            public readonly NodeKind kind;
            public readonly object value;
        }

        public struct Ast
        {
            public Ast(List<Node> nodes)
            {
                this.nodes = nodes;
                this.kind = NodeKind.VttAst;
            }

            public readonly NodeKind kind;
            public readonly List<Node> nodes;

            public string Dump()
            {
                string result = "";

                foreach (Node node in this.nodes)
                {
                    result += $"{node.kind.ToString()}\n";
                    result += $"{node.value.ToString()}\n";
                    result += $"==================\n";
                }

                return result;
            }
        }

        public static List<Node> GetUnknowns(Ast ast)
        {
            return ast.nodes.Where(node => node.kind == NodeKind.Unknown).ToList();
        }

        private int ParseTimestampFromIndex(List<Lexer.Token> tokens, int index)
        {
            int tCursor = index;
            Lexer.Token tCurrent = tokens.ElementAt(tCursor);
            string timestamp = "";

            // Keep going until we see a space or newline
            while (
                tCurrent.type != Lexer.TokenType.Space
                && tCurrent.type != Lexer.TokenType.Newline
            ) {
                tCursor++;
                tCurrent = tokens.ElementAt(tCursor);
                timestamp = $"{timestamp}{tCurrent.value}";
            }

            // timestamp format: 00:05:00,400
            // hours:minutes:seconds,milliseconds
            string[] parts = timestamp.Split('.');

            // If the timestamp is invalid, we return zero to make the instruction runner
            // ignore this screen.
            if (parts.Length != 2)
            {
                throw new ArgumentException($"Cannot parse timestamp {timestamp}, because it cannot be split into two parts on a full stop");
            }

            string time = parts[0];
            int millis = 0;
            int.TryParse(parts[1], out millis);

            string[] timeParts = time.Split(':');

            // If the timestamp is invalid, we return zero to make the instruction runner
            // ignore this screen.
            if (timeParts.Length != 3)
            {
                throw new ArgumentException($"Cannot parse timestamp {timestamp}, because it cannot be split into three parts on a colon");
            }

            int hours = 0;
            int minutes = 0;
            int seconds = 0;

            int.TryParse(timeParts[0], out hours);
            int.TryParse(timeParts[1], out minutes);
            int.TryParse(timeParts[2], out seconds);

            // Add values to get the value in millis
            return millis + (seconds * 1000) + (minutes * 60 * 1000) + (hours * 60 * 60 * 1000);
        }

        public Ast Parse(List<Lexer.Token> tokens)
        {
            List<Node> nodes = new List<Node>();
            int cursor = 0;

            // Mode is just a NodeKind, to check where we currently are
            // The first thing in a .vtt should be the VTT header.
            NodeKind mode = NodeKind.Header;

            while (cursor < tokens.Count)
            {
                // Console.WriteLine($"Parsing... cursor: {cursor}, mode: {mode}");

                Lexer.Token current = tokens.ElementAt(cursor);

                // Ignore notes as they're just comments
                if (current.type == Lexer.TokenType.Note)
                {
                    cursor++;
                    continue;
                }

                // Ignore styles (for now) as they're probably hard to convert into
                // TMPro styles.
                if (current.type == Lexer.TokenType.Style)
                {
                    cursor++;
                    continue;
                }

                if (mode == NodeKind.Header)
                {
                    if (current.type != Lexer.TokenType.WEBVTTHeader)
                    {
                        cursor++;
                        continue;
                    }

                    mode = NodeKind.TimestampStart;
                    cursor++;
                    continue;
                }

                // If we're in the start timestamp
                if (mode == NodeKind.TimestampStart)
                {
                    // Go until we see an int
                    // token type 2 == int
                    if (current.type != Lexer.TokenType.Number)
                    {
                        cursor++;
                        continue;
                    }

                    // Special case to ignore the screen number if present, as this VTT parser just uses its
                    // index
                    if ((cursor + 1) < tokens.Count && tokens[cursor + 1].type == Lexer.TokenType.Newline) {
                        cursor++;
                        continue;
                    }

                    int timestampMillis = -1;

                    try
                    {
                        timestampMillis = this.ParseTimestampFromIndex(tokens, cursor);
                    } catch (ArgumentException ex)
                    {
                        Node unknownNode = new Node(NodeKind.Unknown, ex.Message);
                        nodes.Add(unknownNode);
                        mode = NodeKind.TimestampArrow;
                        continue;
                    }

                    // node kind 2 == TimestampStart
                    Node node = new Node(NodeKind.TimestampStart, timestampMillis);
                    nodes.Add(node);

                    // Skip the timestamp + a space
                    cursor = cursor + 12;
                    // Move to expecting the arrow
                    mode = NodeKind.TimestampArrow;
                    continue;
                }

                // Expect the arrow
                if (mode == NodeKind.TimestampArrow)
                {
                    // Go until we see a hyphen
                    if (current.type != Lexer.TokenType.Hyphen)
                    {
                        cursor++;
                        continue;
                    }

                    int tCursor = cursor;
                    Lexer.Token tCurrent = tokens.ElementAt(tCursor);
                    string body = "";

                    // Keep going until we see a space
                    while (tCurrent.type != Lexer.TokenType.Space && tCurrent.type != Lexer.TokenType.Unknown)
                    {
                        body = $"{body}{tCurrent.value}";
                        tCursor++;
                        tCurrent = tokens.ElementAtOrDefault(tCursor);

                        if (Object.Equals(tCurrent, default))
                        {
                            break;
                        }
                    }

                    // If we saw a valid arrow, add it as a Node, then move the cursor behind it
                    if (body == "-->")
                    {
                        // node kind 3 == TimestampArrow
                        Node node = new Node(NodeKind.TimestampArrow, body);
                        nodes.Add(node);

                        cursor = tCursor;
                    }
                    // If we didn't, advance the cursor by one and add an unknown node
                    else
                    {
                        Node unknownNode = new Node(NodeKind.Unknown, $"Cannot parse arrow: {current.value}");
                        nodes.Add(unknownNode);
                        cursor++;
                    }

                    // Move to expecting the second timestamp, even if we didn't see an arrow
                    // to prevent infinite loops.
                    mode = NodeKind.TimestampEnd;
                    continue;
                }

                // Expect the end timestamp
                if (mode == NodeKind.TimestampEnd)
                {
                    // Go until we see an int
                    // token type 2 == int
                    if (current.type != Lexer.TokenType.Number)
                    {
                        cursor++;
                        continue;
                    }

                    int timestampMillis = -1;

                    try
                    {
                        timestampMillis = this.ParseTimestampFromIndex(tokens, cursor);
                    }
                    catch (ArgumentException ex)
                    {
                        Node unknownNode = new Node(NodeKind.Unknown, ex.Message);
                        nodes.Add(unknownNode);
                        mode = NodeKind.TimestampArrow;
                        continue;
                    }

                    // node kind 4 == TimestampEnd
                    Node node = new Node(NodeKind.TimestampEnd, timestampMillis);
                    nodes.Add(node);

                    // Skip the timestamp + a space
                    cursor = cursor + 12;
                    // Move to expecting the arrow
                    mode = NodeKind.Parameters;
                    continue;
                }

                if (mode == NodeKind.Parameters)
                {
                    if (current.type == Lexer.TokenType.Newline)
                    {
                        cursor++;
                        mode = NodeKind.TextContents;
                        continue;
                    }

                    string allParameters = "";

                    while (cursor < tokens.Count && tokens[cursor].type != Lexer.TokenType.Newline)
                    {
                        allParameters += tokens[cursor].value;
                        cursor++;
                    }

                    string[] stringParameters = allParameters.Split(' ');
                    Dictionary<string, string> parameters = new Dictionary<string, string>();

                    foreach (string parameter in stringParameters)
                    {
                        string[] parts = parameter.Split(':');
                        if (parts.Length != 2) continue;

                        string name = parts[0];
                        string value = parts[1];

                        parameters.Add(name, value);
                    }

                    Node node = new Node(NodeKind.Parameters, parameters);
                    nodes.Add(node);
                    mode = NodeKind.TextContents;
                    continue;
                }

                // Expect the text contents
                if (mode == NodeKind.TextContents)
                {
                    int tCursor = cursor;
                    string textContents = "";

                    while (tCursor < tokens.Count && tokens.ElementAt(tCursor).type != Lexer.TokenType.DoubleNewline)
                    {
                        Lexer.Token tCurrent = tokens.ElementAt(tCursor);
                        textContents = $"{textContents}{tCurrent.value}";
                        tCursor++;
                    }

                    if (textContents == "")
                    {
                        Node unknownNode = new Node(NodeKind.Unknown, $"Cannot parse text contents in token {tCursor} because the parsed value is empty");
                        nodes.Add(unknownNode);
                    } else
                    {
                        Node node = new Node(NodeKind.TextContents, textContents);
                        nodes.Add(node);
                    }

                    cursor = tCursor;
                    // Go back to expecting the next section's index
                    mode = NodeKind.TimestampStart;
                    continue;
                }

                // If none of the modes did anything, we move the cursor forward by one to try to salvage the source file.
                // Screens or other data might be missing but at least most of the file will be parsed still
                cursor++;
            }

            return new Ast(nodes);
        }
    }
}
