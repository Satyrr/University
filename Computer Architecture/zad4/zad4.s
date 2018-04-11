	.text
	.globl	fibonacci
	.type	fibonacci, @function
fibonacci:
	pushq 	%rbp
	movq 	%rsp, %rbp
	cmpq	$0, %rdi    #fibonacci(0)
	je		.LEnd0
	cmpq	$1, %rdi    #fibonacci(1)
	je		.LEnd1
	pushq	%rdi
	decq	%rdi
	call	fibonacci
	popq	%rdi
	push	%rax
	subq	$2, %rdi
	call 	fibonacci
	popq	%rcx
	addq	%rcx, %rax  #fibonacci(n-1) + fibonacci(n-2)
	jmp	.LEnd
.LEnd0:
	movq	$0, %rax    #fibonacci(0)
	jmp .LEnd
.LEnd1:
	movq	$1, %rax    #fibonacci(1)
.LEnd:
	popq 	%rbp
	ret
.size fibonacci, .-fibonacci	
