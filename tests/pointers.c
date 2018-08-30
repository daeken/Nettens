typedef unsigned int uint;

uint deref(void *ptr) {
	uint a = *(uint *) ptr;
	return a + 42;
}

uint test() {
	void *ptr = (void *) 0xdeadbeef;
	return deref(ptr);
}
