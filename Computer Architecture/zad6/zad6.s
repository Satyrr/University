.file	"zad6.s"	
.section	.rodata
napis1:
	.string "Podaj ilosc liczb:"
napis2:
	.string "%d"
napis3:
	.string "%s"
napis4:
	.string "min: %d, max:%d\n"
	.text
	.globl	main
	.type	main, @function
main:
	pushq 	%rbp
	movq 	%rsp, %rbp
	movq	$napis1, %rdi		#format dla printf
	xorl	%eax, %eax			#wyzerowanie eax przed wywolaniem printf
	call 	printf	
	movq	$napis2, %rdi		#format dla scanf
	subq	$16, %rsp			#wyrowananie rsp do 16 przed wywolaniem funkcji
	movq	%rsp, %rsi			#zapisanie wartosci n(ilosci liczb w ciagu) na szczycie stosu
	call 	__isoc99_scanf
	movq	(%rsp), %rcx		#%rcx = n
	subq	$128, %rsp			#miejsce na ciag znakow
.L1:	
	cmpq	$0, %rcx			#sprawdzenie, czy podano wszystkie liczby
	je		.LMin
	movq	$napis3, %rdi		
	leaq	-144(%rbp), %rax	#miejsce w stosie, do ktorego sa wpisywane lancuchy
	movq	%rax, %rsi
	movq	$0,	%rax			#wyzerowanie raxa dla scanf
	subq	$8, %rsp			#zachowanie rcx i wyrownanie stosu do 16 bajtow
	pushq	%rcx
	call 	__isoc99_scanf
	popq	%rcx				#odzyskanie rcx
	addq	$8, %rsp
	leaq	-144(%rbp), %rax
	movq	%rax, %rdi			#argument dla atol
	subq	$8, %rsp			#wyrownanie stosu
	pushq	%rcx				#zachowanie rcx
	call 	atol
	popq	%rcx
	addq	$8, %rsp
	subq	$16, %rsp			#miejsce na kolejna liczbe ciagu
	movq	%rax, (%rsp)
	decq 	%rcx
	jmp 	.L1					#na stosie znajduja sie kolejne wprowadzone liczby

.LMin:
	movq	(%rsp), %rdx		#ciag[last]=min
	decq	%rcx				#index = -1
.L2:
	incq	%rcx				#index ++
	cmpq	-16(%rbp), %rcx		#if(index=max_index) goto wyznaczanie max
	je		.LMax
	movq	%rcx, %rax
	addq	%rax, %rax
	cmpq	(%rsp,%rax,8),%rdx	#if(ciag[index]<min) min = ciag[index]
	js		.L2
	movq	(%rsp,%rax,8),%rdx	
	jmp		.L2
	
.LMax:
	movq	%rdx, %rsi			#rsi = argument dla koncowego printf
	movq	$-1, %rcx
	movq	(%rsp), %rdx
.L3:
	incq	%rcx
	cmpq	-16(%rbp), %rcx
	je		.LEnd
	movq	%rcx, %rax
	addq	%rax, %rax
	cmpq	(%rsp,%rax,8),%rdx
	jns		.L3
	movq	(%rsp,%rax,8),%rdx	
	jmp		.L3
.LEnd:
	movq	$napis4, %rdi
	movq	$0, %rax
	call 	printf
	movq	$0, %rax
	leave
	ret
.size main, .-main	
