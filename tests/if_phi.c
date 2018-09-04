int dbl(int b) {
	return b + b;
}

int test(int a, int b) {
	int c = a + b;
	if(a > 0)
		c /= a;
	c += dbl(b);
	return c;
}