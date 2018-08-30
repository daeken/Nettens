typedef unsigned int uint;

typedef struct foo_s {
	uint a, b;
	void *c;
	struct foo_s *d;
} foo_t;

void test() {
	foo_t foo;
	foo.a = 5;
	foo.c = &foo;
	foo.b = ((foo_t *) foo.c)->a + 6;
	foo.d = (foo_t *) foo.c;
}
