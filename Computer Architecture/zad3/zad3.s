	.text
	.globl	insert_sort
	.type	insert_sort, @function
first=%rdi
last=%rsi
index=%rcx
insert_sort:
	pushq 	%rbp
	movq 	%rsp, %rbp
	pushq	%rbx
	movq	$0, index	#index=0
.L1:
	incq	index		#index++
	leaq	(first,%rcx,8), %rbx		#czy index>max_index?
	cmpq	%rbx, last
	js		.LEnd
	movq	(first,%rcx,8),%rax		#rax=tab[index]
	movq	index, %rdx				#rdx=temp index
	jmp 	.L2
.L4:
	movq	(first,%rdx,8), %rbx		# jezeli tab[rdx]>tab[index], przesun tab[rdx] w prawo
	movq	%rbx,8(first,%rdx,8)
.L2:
	cmpq	$0, %rdx				#czy temp index==0 
	je		.L3
	decq	%rdx
	cmpq	(first,%rdx,8),%rax     #czy tab[rdx]>tab[index]
	js		.L4
	incq	%rdx
.L3:
	movq	%rax,(first,%rdx,8)	    #wstawienie w ciag uporzadkowany
	jmp		.L1
.LEnd:
	popq	%rbx
	popq 	%rbp
	ret
.size insert_sort, .-insert_sort	
