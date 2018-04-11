.file	"zad7.s"	
.section	.bss
.Lbuff:
	.lcomm bufor, 32		#32 bajtowy bufor 
	.text
	.globl	main
	.type	main, @function
main:
	pushq 	%rbp
	movq 	%rsp, %rbp
.LRead:
	movq	$0, %rax
	movq	$0, %rdi
	movq	$.Lbuff, %rsi
	movq	$32, %rdx
	syscall					#wczytanie znakow do bufora, w rax ilosc wczytanych bajtow
	pushq	%rax
	movq	$0, %rcx
	jmp		.L1
.L2:
	incq	%rcx
.L1:
	cmpq	%rcx, %rax		#jesli zmieniono rozmiar wszystkich znakow, skocz do Write
	je		.LWrite
	movq	$.Lbuff, %rsi
	movb	(%rsi,%rcx,1), %dh		#iteracja po buforze i zmiana znakow na mniejsze/wieksze
	cmpb	$96, %dh
	js		.LUpper
	subb	$32, %dh
	movb	%dh, (%rsi,%rcx,1)
	jmp		.L2
.LUpper:
	addb	$32, %dh
	movb	%dh, (%rsi,%rcx,1)
	jmp		.L2
.LWrite:
	movq	$1, %rax		#wypisanie znakow
	movq	$1, %rdi
	movq	$.Lbuff, %rsi	
	movq	(%rsp), %rdx
	decq	%rdx
	syscall
.LEnd:
	leave
	ret
.size main, .-main	
