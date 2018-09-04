SRC = $(wildcard *.c)

ASMO0 = $(SRC:.c=.O0.s)
ASMO1 = $(SRC:.c=.O1.s)
BCO0 = $(SRC:.c=.O0.bc)
BCO1 = $(SRC:.c=.O1.bc)
MFIRO0 = $(SRC:.c=.O0.mfir)
MFIRO1 = $(SRC:.c=.O1.mfir)

CCFLAGS = -c -emit-llvm -target i386-pc-none-eabi

%.O0.s: %.c
	clang -S $(CCFLAGS) -O0 -o $@ $<

%.O1.s: %.c
	clang -S $(CCFLAGS) -O1 -o $@ $<

%.O0.bc: %.c
	clang $(CCFLAGS) -O0 -o $@ $<

%.O1.bc: %.c
	clang $(CCFLAGS) -O1 -o $@ $<

%.mfir: %.bc
	../Ir2Sane/cmake-build-debug/Ir2Sane $< > $@

all: $(ASMO0) $(ASMO1) $(BCO0) $(BCO1) $(MFIRO0) $(MFIRO1)

clean:
	rm *.s
	rm *.bc
	rm *.mfir
