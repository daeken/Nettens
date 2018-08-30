SRC = $(wildcard *.c)

ASM = $(SRC:.c=.s)
BC = $(SRC:.c=.bc)

CCFLAGS = -c -emit-llvm -target i386-pc-none-eabi

%.s: %.c
	clang -S $(CCFLAGS) -o $@ $<

%.bc: %.c
	clang $(CCFLAGS) -o $@ $<

all: $(ASM) $(BC)

clean:
	rm *.s
	rm *.bc
