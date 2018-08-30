SRC = $(wildcard *.c)

ASM = $(SRC:.c=.s)
BC = $(SRC:.c=.bc)

%.s: %.c
	clang -c -S -emit-llvm -o $@ $<

%.bc: %.c
	clang -c -emit-llvm -o $@ $<

all: $(ASM) $(BC)

clean:
	rm *.s
	rm *.bc
