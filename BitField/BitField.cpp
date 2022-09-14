#define _CRT_SECURE_NO_WARNINGS
#include <iostream>
#include <conio.h>
union OneByte
{
    unsigned char byte;
    struct
    {
        unsigned bit1 : 1;
        unsigned bit2 : 1;
        unsigned bit3 : 1;
        unsigned bit4 : 1;
        unsigned bit5 : 1;
        unsigned bit6 : 1;
        unsigned bit7 : 1;
        unsigned bit8 : 1;
    } bits;

    void print()
    {
        printf("\n%d%d%d%d%d%d%d%d(b) = %02X(h) = %d(d)"            
            , bits.bit8
            , bits.bit7
            , bits.bit6
            , bits.bit5
            , bits.bit4
            , bits.bit3
            , bits.bit2
            , bits.bit1
            , byte
            , byte);
    }

    void toggle_bit(char bit)
    {
        byte = byte xor (1 << (bit-1));
    }

    void on_bit(char bit)
    {
        byte = byte | (1 << (bit - 1));
    }

    void off_bit(char bit)
    {
        byte = byte & ~(1 << (bit - 1));
    }

    void invert_bits()
    {
        byte = ~byte;
    }
};

unsigned char get_byte()
{
    int i;
    do
    {
        printf("Введите лчисло от 0 до 255: ");
    } while ((scanf("%i", &i) < 1) || (i < 0) || (i > 255));
    
    return i;
}

int get_oper()
{
    int oper = -1;
    do
    {
        printf("\nВыключить бит - 0, включить бит - 1, переключить бит - 2, инвертировать биты - 3, выйти - 9: ");
        oper = _getche() - 48;

        if (oper == 9)
            return -1;
        else if (oper >= 0 && oper <= 3)
            return oper;
        
        printf("\nневерная операция");
    } while (true);
}

int get_bit_num()
{
    int bit_num = -1;
    do
    {
        printf("\nВведите номер бита (1-8): ");
        bit_num = _getche() - 48;

        if (bit_num >= 1 && bit_num <= 8)
            return bit_num;

        printf("\nневерный номер бита");
    } while (true);
}

int main()
{
    setlocale(LC_ALL, "Russian");

    OneByte b = { get_byte() };
    b.print();

    while (true)
    {
        switch (get_oper())
        {
            case -1:
                return 0;
            case 0:
                b.off_bit(get_bit_num());
                break;
            case 1:
                b.on_bit(get_bit_num());
                break;
            case 2:
                b.toggle_bit(get_bit_num());
                break;
            case 3: 
                b.invert_bits();
                break;
        }
        
        b.print();
    }
}