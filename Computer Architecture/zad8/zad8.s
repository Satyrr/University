.file	"zad8.s"	
.section	.rodata
.Lconst2:
	.long	0
	.long	1071644672

	.text
	.globl	approx_sqrt
	.type	approx_sqrt, @function

arg0=%xmm0
epsilon=%xmm1
xn=%xmm2
xn1=%xmm3
temp=%xmm4
temp2=%xmm5
const2=%xmm6

approx_sqrt:
	pushq 	%rbp
	movq 	%rsp, %rbp
	movsd	arg0, xn
	movsd	xn, temp
	movsd	.Lconst2(%rip), const2

.Lcmp:
	ucomisd	epsilon, temp	
	jb	.Lend

	movsd	arg0, xn1
	divsd	xn, xn1
	addsd	xn, xn1
	mulsd	const2, xn1
	movsd	xn,	temp
	subsd	xn1, temp
	jb		.Lmod
	jmp		.Lxn
.Lmod:
	movsd	temp, temp2
	subsd	temp2, temp
	subsd	temp2, temp
.Lxn:
	movsd	xn1, xn
	jmp		.Lcmp
.Lend:
	movsd	xn1, %xmm0
	leave
	ret
.size approx_sqrt, .-approx_sqrt	
