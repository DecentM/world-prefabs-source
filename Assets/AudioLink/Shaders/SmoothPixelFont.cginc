// Temporary defines for testing only
#define __0             48
#define __1             49
#define __2             50
#define __3             51
#define __4             52
#define __5             53
#define __6             54
#define __7             55
#define __8             56
#define __9             57
#define __SPACE         32
#define __EXCLAMATION   33
#define __QUOTE         34
#define __HASH          35
#define __DOLLAR        36
#define __AMP           38
#define __APOSTROPHE    39
#define __PAREN_OPEN    40
#define __PAREN_CLOSED  41
#define __MULT          42
#define __PLUS          43
#define __COMMA         44
#define __DASH          45
#define __PERIOD        46
#define __FWD_SLASH     47
#define __COLON         58
#define __SEMICOLON     59
#define __LESSTHAN      60
#define __EQUAL         61
#define __GREATERTHAN   62
#define __QUESTION      63
#define __CARROT        94
#define __A             65
#define __B             66
#define __C             67
#define __D             68
#define __E             69
#define __F             70
#define __G             71
#define __H             72
#define __I             73
#define __J             74
#define __K             75
#define __L             76
#define __M             77
#define __N             78
#define __O             79
#define __P             80
#define __Q             81
#define __R             82
#define __S             83
#define __T             84
#define __U             85
#define __V             86
#define __W             87
#define __X             88
#define __Y             89
#define __Z             90
#define __a             97
#define __b             98
#define __c             99
#define __d             100
#define __e             101
#define __f             102
#define __g             103
#define __h             104
#define __i             105
#define __j             106
#define __k             107
#define __l             108
#define __m             109
#define __n             110
#define __o             111
#define __p             112
#define __q             113
#define __r             114
#define __s             115
#define __t             116
#define __u             117
#define __v             118
#define __w             119
#define __x             120
#define __y             121
#define __z             122
#define __UNDERSCORE    95

// Smooth pixel font bitmap
const static uint2 bitmapFont[96] = {
    {        0,         0 }, // 0  32 ' '
    {  4472896,   4472896 }, // 1  33 '!' // 0100 0100 0100 0000 0100 0000      0100 0100 0100 0000 0100 0000  
    { 11141120,  11141120 }, // 2  34 '"' // 1010 1010 0000 0000 0000 0000      1010 1010 0000 0000 0000 0000
    { 11447968,  11447968 }, // 3  35 '#' // 1010 1110 1010 1110 1010 0000      1010 1110 1010 1110 1010 0000
    {  5162720,   5162724 }, // 4  36 '$' // 0100 1110 1100 0110 1110 0000      0100 1110 1100 0110 1110 0100
    {        0,         0 }, // 5  37 '%' // NOT WRITTEN
    {  4868704,  15395552 }, // 6  38 '&' // 0100 1010 0100 1010 0110 0000      1110 1010 1110 1010 1110 0000
    {  4456448,   4456448 }, // 7  39 ''' // 0100 0100 0000 0000 0000 0000      1110 1010 1110 1010 1110 0000
    {  2376736,   6571104 }, // 8  40 '(' // 0010 0100 0100 0100 0010 0000      0110 0100 0100 0100 0110 0000
    {  8668288,  12862656 }, // 9  41 ')' // 1000 0100 0100 0100 1000 0000      1100 0100 0100 0100 1100 0000
    {   674304,    978432 }, // 10 42 '*' // 0000 1010 0100 1010 0000 0000      0000 1110 1110 1110 0000 0000
    {   320512,    320512 }, // 11 43 '+' // 0000 0100 1110 0100 0000 0000      0000 0100 1110 0100 0000 0000
    {     1088,      1228 }, // 12 44 ',' // 0000 0000 0000 0100 0100 0000      0000 0000 0000 0100 1100 1100
    {    57344,     57344 }, // 13 45 '-' // 0000 0000 1110 0000 0000 0000      0000 0000 1110 0000 0000 0000
    {       64,        64 }, // 14 46 '.' // 0000 0000 0000 0000 0100 0000      0000 0000 0000 0000 0100 0000
    {  2246784,   2287744 }, // 15 47 '/' // 0010 0010 0100 1000 1000 0000      0010 0010 1110 1000 1000 0000
    {  6990528,  15379168 }, // 16 48 '0' // 0110 1010 1010 1010 1100 0000      1110 1010 1010 1010 1110 0000
    {  4998368,   4998368 }, // 17 49 '1' // 0100 1100 0100 0100 1110 0000      0100 1100 0100 0100 1110 0000
    { 14870752,  14870752 }, // 18 50 '2' // 1110 0010 1110 1000 1110 0000      1110 0010 1110 1000 1110 0000
    { 14828256,  14836448 }, // 19 51 '3' // 1110 0010 0100 0010 1110 0000      1110 0010 0110 0010 1110 0000
    {  9101856,   9101856 }, // 20 52 '4' // 1000 1010 1110 0010 0010 0000      1000 1010 1110 0010 0010 0000
    { 15262432,  15262432 }, // 21 53 '5' // 1110 1000 1110 0010 1110 0000      1110 1000 1110 0010 1110 0000
    {  6875872,  15264480 }, // 22 54 '6' // 0110 1000 1110 1010 1110 0000      1110 1000 1110 1010 1110 0000
    { 14829120,  14836800 }, // 23 55 '7' // 1110 0010 0100 0110 0100 0000      1110 0010 0110 0100 0100 0000
    { 15395552,  15395552 }, // 24 56 '8' // 1110 1010 1110 1010 1110 0000      1110 1010 1110 1010 1110 0000
    { 15393472,  15393504 }, // 25 57 '9' // 1110 1010 1110 0010 1100 0000      1110 1010 1110 0010 1110 0000
    {   263168,    263168 }, // 26 58 ':' // 0000 0100 0000 0100 0000 0000      0000 0100 0000 0100 0000 0000
    {   263232,    263244 }, // 27 59 ';' // 0000 0100 0000 0100 0100 0000      0000 0100 0000 0100 0100 1100
    {  2393120,   7261792 }, // 28 60 '<' // 0010 0100 1000 0100 0010 0000      0110 1110 1100 1110 0110 0000
    {   921088,    921088 }, // 29 61 '=' // 0000 1110 0000 1110 0000 0000      0000 1110 0000 1110 0000 0000
    {  8660096,  13528768 }, // 30 62 '>' // 1000 0100 0010 0100 1000 0000      1100 1110 0110 1110 1100 0000
    { 12730432,  14836800 }, // 31 63 '?' // 1100 0010 0100 0000 0100 0000      1110 0010 0110 0100 0100 0000
    {        0,         0 }, // 32 64 '@' // NOT WRITTEN
    { 15395488,  15395488 }, // 33 65 'A' // 1110 1010 1110 1010 1010 0000      1110 1010 1110 1010 1010 0000
    { 15387360,  15395552 }, // 34 66 'B' // 1110 1010 1100 1010 1110 0000      1110 1010 1110 1010 1110 0000
    { 15239392,  15239392 }, // 35 67 'C' // 1110 1000 1000 1000 1110 0000      1110 1000 1000 1000 1110 0000
    { 13281984,  15379168 }, // 36 68 'D' // 1100 1010 1010 1010 1100 0000      1110 1010 1010 1010 1110 0000
    { 15255776,  15255776 }, // 37 69 'E' // 1110 1000 1100 1000 1110 0000      1110 1000 1100 1000 1110 0000
    { 15255680,  15255680 }, // 38 70 'F' // 1110 1000 1100 1000 1000 0000      1110 1000 1100 1000 1000 0000
    { 15248096,  15248096 }, // 39 71 'G' // 1110 1000 1010 1010 1110 0000      1110 1000 1010 1010 1110 0000
    { 11201184,  11201184 }, // 40 72 'H' // 1010 1010 1110 1010 1010 0000      1010 1010 1110 1010 1010 0000
    { 14959840,  14959840 }, // 41 73 'I' // 1110 0100 0100 0100 1110 0000      1110 0100 0100 0100 1110 0000
    {  2239200,   2239200 }, // 42 74 'J' // 0010 0010 0010 1010 1110 0000      0010 0010 0010 1010 1110 0000
    { 11192992,  11201184 }, // 43 75 'K' // 1010 1010 1100 1010 1010 0000      1010 1010 1110 1010 1010 0000
    {  8947936,   8947936 }, // 44 76 'L' // 1000 1000 1000 1000 1110 0000      1000 1000 1000 1000 1110 0000
    { 11463328,  15657632 }, // 45 77 'M' // 1010 1110 1110 1010 1010 0000      1110 1110 1110 1010 1010 0000
    { 13281952,  15379104 }, // 46 78 'N' // 1100 1010 1010 1010 1010 0000      1110 1010 1010 1010 1010 0000
    { 15379168,  15379168 }, // 47 79 'O' // 1110 1010 1010 1010 1110 0000      1110 1010 1010 1010 1110 0000
    { 15394944,  15394944 }, // 48 80 'P' // 1110 1010 1110 1000 1000 0000      1110 1010 1110 1000 1000 0000
    { 15379040,  15379168 }, // 49 81 'Q' // 1110 1010 1010 1010 0110 0000      1110 1010 1010 1010 1110 0000
    { 15387296,  15395488 }, // 50 82 'R' // 1110 1010 1100 1010 1010 0000      1110 1010 1110 1010 1010 0000
    {  6873792,  15262432 }, // 51 83 'S' // 0110 1000 1110 0010 1100 0000      1110 1000 1110 0010 1110 0000
    { 14959680,  14959680 }, // 52 84 'T' // 1110 0100 0100 0100 0100 0000      1110 0100 0100 0100 0100 0000
    { 11184736,  11184864 }, // 53 85 'U' // 1010 1010 1010 1010 0110 0000      1010 1010 1010 1010 1110 0000
    { 11445472,  11202112 }, // 54 86 'V' // 1010 1110 1010 0100 1110 0000      1010 1010 1110 1110 0100 0000
    { 11202208,  11202272 }, // 55 87 'W' // 1010 1010 1110 1110 1010 0000      1010 1010 1110 1110 1110 0000
    { 11160224,  11201184 }, // 56 88 'X' // 1010 1010 0100 1010 1010 0000      1010 1010 1110 1010 1010 0000
    { 15352896,  11420736 }, // 57 89 'Y' // 1110 1010 0100 0100 0100 0000      1010 1110 0100 0100 0100 0000
    { 14829792,  14870752 }, // 58 90 'Z' // 1110 0010 0100 1000 1110 0000      1110 0010 1110 1000 1110 0000
    {        0,         0 }, // 59 91 '[' // NOT WRITTEN
    {        0,         0 }, // 60 92 '\' // NOT WRITTEN
    {        0,         0 }, // 61 93 ']' // NOT WRITTEN
    {  4849664,  15597568 }, // 62 94 '^' // 0100 1010 0000 0000 0000 0000      1110 1110 0000 0000 0000 0000
    {      224,       224 }, // 63 95 '_' // 0000 0000 0000 0000 1110 0000      0000 0000 0000 0000 1110 0000
    {        0,         0 }, // 64 96 '`' // NOT WRITTEN
    {   436832,    961248 }, // 65 97 'a' // 0000 0110 1010 1010 0110 0000      0000 1110 1010 1010 1110 0000
    {  9349856,   9349856 }, // 66 98 'b' // 1000 1110 1010 1010 1110 0000      1000 1110 1010 1010 1110 0000
    {   952544,    952544 }, // 67 99 'c' // 0000 1110 1000 1000 1110 0000      0000 1110 1000 1000 1110 0000
    {  3058400,   3058400 }, // 68 100 'd' // 0010 1110 1010 1010 1110 0000      0010 1110 1010 1010 1110 0000
    {   961760,    962272 }, // 69 101 'e' // 0000 1110 1010 1100 1110 0000      0000 1110 1010 1110 1110 0000
    {  6612032,   6612032 }, // 70 102 'f' // 0110 0100 1110 0100 0100 0000      0110 0100 1110 0100 0100 0000
    {   976608,    962272 }, // 71 103 'g' // 0000 1110 1110 0110 1110 0000      0000 1110 1010 1110 1110 0000
    {  9349792,   9349792 }, // 72 104 'h' // 1000 1110 1010 1010 1010 0000      1000 1110 1010 1010 1010 0000
    {  4474080,   4867296 }, // 73 105 'i' // 0100 0100 0100 0100 1110 0000      0100 1010 0100 0100 1110 0000
    {  2239200,   2435808 }, // 74 106 'j' // 0010 0010 0010 1010 1110 0000      0010 0101 0010 1010 1110 0000
    {  9096352,   9105056 }, // 75 107 'k' // 1000 1010 1100 1100 1010 0000      1000 1010 1110 1110 1010 0000
    {  4474080,  12862688 }, // 76 108 'l' // 0100 0100 0100 0100 1110 0000      1100 0100 0100 0100 1110 0000
    {   715424,    977568 }, // 77 109 'm' // 0000 1010 1110 1010 1010 0000      0000 1110 1110 1010 1010 0000
    {   830112,    961184 }, // 78 110 'n' // 0000 1100 1010 1010 1010 0000      0000 1110 1010 1010 1010 0000
    {   961248,    961248 }, // 79 111 'o' // 0000 1110 1010 1010 1110 0000      0000 1110 1010 1010 1110 0000
    {   962176,    962176 }, // 80 112 'p' // 0000 1110 1010 1110 1000 0000      0000 1110 1010 1110 1000 0000
    {   962080,    962080 }, // 81 113 'q' // 0000 1110 1010 1110 0010 0000      0000 1110 1010 1110 0010 0000
    {   714880,    968832 }, // 82 114 'r' // 0000 1010 1110 1000 1000 0000      0000 1110 1100 1000 1000 0000
    {   968416,    968416 }, // 83 115 's' // 0000 1110 1100 0110 1110 0000      0000 1110 1100 0110 1110 0000
    {  5129280,   5129280 }, // 84 116 't' // 0100 1110 0100 0100 0100 0000      0100 1110 0100 0100 0100 0000
    {   699104,    699104 }, // 85 117 'u' // 0000 1010 1010 1010 1110 0000      0000 1010 1010 1010 1110 0000
    {   715328,    700128 }, // 86 118 'v' // 0000 1010 1110 1010 0100 0000      0000 1010 1010 1110 1110 0000
    {   700064,    700128 }, // 87 119 'w' // 0000 1010 1010 1110 1010 0000      0000 1010 1010 1110 1110 0000
    {   672928,    716448 }, // 88 120 'x' // 0000 1010 0100 0100 1010 0000      0000 1010 1110 1110 1010 0000
    {   713312,    713440 }, // 89 121 'y' // 0000 1010 1110 0010 0110 0000      0000 1010 1110 0010 1110 0000
    {   945376,    945376 }, // 90 122 'z' // 0000 1110 0110 1100 1110 0000      0000 1110 0110 1100 1110 0000
    {        0,         0 }, // 91 123 '{' // NOT WRITTEN
    {        0,         0 }, // 92 124 '|' // NOT WRITTEN
    {        0,         0 }, // 93 125 '}' // NOT WRITTEN
    {        0,         0 }, // 94 126 '~' // NOT WRITTEN
    {        0,         0 } // 95 127 FREEBIE!!! // NOT WRITTEN
    };



////////////////////////////////////////////////////////////////////
// General debug functions below here

// Shockingly, including the ability to render text doesn't
// slow down number printing if text isn't used.
// A basic versino of the debug screen without text was only 134
// instructions.

float PrintChar(uint charNum, float2 charUV, float2 softness, float offset)
{
    // .x = 15% .y = 35% added, it's 1.0. ( 0 1 would be 35% )
    charUV += float2(0, 0.5);
    uint2 bitmap = bitmapFont[charNum-32];
    uint4 bitmapA = bitmap.xxxx;
    uint4 bitmapB = bitmap.yyyy;
    uint2 pixel = charUV;
    uint index = pixel.x + pixel.y * 4 - 4;
    uint4 shift = uint4(0, 1, 4, 5) + index;
    uint4 bitSelect = uint4(1, 1, 1, 1);
    bitmapA = (bitmapA >> shift) & bitSelect;
    bitmapB = (bitmapB >> shift) & bitSelect;
    float4 neighbors = (bitmapB & 1) ? (bitmapA ? 1 : 0.35) : (bitmapA ? 0.15 : 0);
    float2 pixelUV = smoothstep(0, 1, frac(charUV));
    float o = lerp(
              lerp(neighbors.x, neighbors.y, pixelUV.x),
              lerp(neighbors.z, neighbors.w, pixelUV.x), pixelUV.y);
    o += offset;
    return saturate(o * softness - softness / 2);
}

// Print a number on a line
//
// value            (float) Number value to display
// charUV           (float2) coordinates on the character to render
// softness
// digit            (uint) Digit in number to render
// digitOffset      (uint) Shift digits to the right
// numFractDigits   (uint) Number of digits to round to after the decimal
//
float PrintNumberOnLine(float value, float2 charUV, float2 softness, uint digit, uint digitOffset, uint numFractDigits, bool leadZero, float offset)
{
    uint charNum;
    uint leadingdash = (value<0)?('-'-'0'):(' '-'0');
    value = abs(value);

    if (digit == digitOffset)
    {
        charNum = __PERIOD;
    }
    else
    {
        int dmfd = (int)digit - (int)digitOffset;
        if (dmfd > 0)
        {
            //fractional part.
            uint fpart = round(frac(value) * pow(10, numFractDigits));
            uint l10 = pow(10.0, numFractDigits - dmfd);
            charNum = ((uint)(fpart / l10)) % 10;
        }
        else
        {
            float l10 = pow(10.0, (float)(dmfd + 1));
            float vnum = value * l10;
            charNum = (uint)(vnum);

            //Disable leading 0's?
            //if (!leadZero && dmfd != -1 && charNum == 0 && dmfd < 0.5)
            //    charNum = ' '-'0'; // space

            if( dmfd < -1 && charNum == 0 )
            {
                
                if( leadZero )
                    charNum %= (uint)10;
                else
                    charNum = leadingdash;
            }
            else
                charNum %= (uint)10;
        }
        charNum += '0';
    }

    return PrintChar(charNum, charUV, softness, offset);
}
