	.text
	.globl	lcm_gcd
	.type	lcm_gcd, @function

arg0=%rdi
arg1=%rsi

lcm_gcd:
	pushq 	%rbp
	movq 	%rsp, %rbp
    cmpq 	arg0, arg1	# wybranie wiekszej liczby; wieksza w arg0
	jb 		.L1
	pushq	arg0
	movq	arg1,arg0
	popq	arg1
.L1:
	pushq 	arg0	#Zapamietanie argumentow: arg0=-8(%rbp) 
	pushq 	arg1	#arg1=-16(%rbp) 
	jmp		.LNWD
.LDecNWD:	
	decq	arg1	
.LNWD:
	movq	-8(%rbp),%rax
	xorq	%rdx, %rdx
	divq	arg1			#arg1/nwd
	testq	%rdx,%rdx		#sprawdzenie, czy arg1%nwd==0
	jne		.LDecNWD	
	movq	-16(%rbp),%rax	
	xorq	%rdx, %rdx
	divq 	arg1			#arg2/nwd
	testq	%rdx,%rdx		#sprawdzenie, czy arg2%nwd==0
	jne		.LDecNWD				
	pushq	arg1	#NWD = -24(%rbp)
	jmp	.LNWW
.LNWWInc:	
	incq	arg0	#NwW
.LNWW:
	movq	arg0,%rax
	xorq	%rdx, %rdx
	divq 	-8(%rbp)	#NWW/arg0
	testq	%rdx,%rdx
	jne		.LNWWInc	
	movq	arg0,%rax
	xorq	%rdx, %rdx
	divq 	-16(%rbp)	#NWW/arg1
	testq	%rdx,%rdx
	jne		.LNWWInc
	movq	arg0, %rax		#NWW
	movq	-24(%rbp),%rdx	#NWD
	addq	$24,%rsp
	popq 	%rbp
	ret
.size lcm_gcd, .-lcm_gcd	
