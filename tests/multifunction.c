int add(int a, int b) {
	return a + b;
}

int mul(int a, int b) {
	return a * b;
}

int fused_muladd(int a, int b, int c) {
	return add(mul(a, b), c);
}

