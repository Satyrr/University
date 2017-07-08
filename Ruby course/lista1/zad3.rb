def podzielniki(n)

	pierwsze = []
	i=2
	nTemp = n
	while i <= nTemp do 
		if n % i == 0 then
			pierwsze.push(i)
			while n % i == 0 
				n /= i
			end
		end
		i+=1
	end
	puts(pierwsze)
end

podzielniki(72)