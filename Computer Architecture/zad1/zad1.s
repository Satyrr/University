	.text
	.globl	clz
	.type	clz, @function

licznik=%rax
mask=%rbx
temp=%rdx
bits=%rcx
bitscl=%cl
clz:
	pushq 	%rbp
	movq 	%rsp, %rbp
	pushq	%rbx
	movq	$64, licznik	#szczegolny przypadek dla 0
	cmpq	$0, %rdi
	je		.LEnd
	movq	$32, bits
	xorq	licznik, licznik	# licznik=0
	movq	$1, mask
	shlq	$63, mask
	sarq	$31, mask	#mask= 1111...0000
	
.L1:
	cmpq	$0, bits
	je 		.LEnd
	movq	mask, temp
	andq	%rdi, temp		#nalozenie maski na argument
	cmpq	$0, temp		#jezeli 0 - przesuniecie maski w prawo
	je		.LRight
	shrq	$1, bits		# w p.p. zmniejszenie maski 
	movq	mask, temp
	shlq	bitscl, temp
	andq	temp, mask
	jmp 	.L1
.LRight:
	addq	bits, licznik
	shrq	bitscl, mask
	jmp .L1

.LEnd:
	popq	%rbx
	popq 	%rbp
	ret
.size clz, .-clz	


;
