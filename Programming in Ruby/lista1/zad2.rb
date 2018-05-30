def pascal(n)

	if n > 0 then puts(1) end
	if n > 1 then 
		print(1)
		print(" ")
		print(1)
		print("\n")
 	end

 	prevRow = [1,1]
 	rowNr = 3

	while rowNr <= n do
		currRow = [1]

		print(1)
		print(" ")

		i = 0
		while i+1 < prevRow.length do
			sum = prevRow[i]+prevRow[i+1]
			print(sum)
			print(" ")
			currRow.push(sum)
			i+=1
		end
		currRow.push(1)
		prevRow = currRow
		rowNr+=1

		print(1)
		print("\n")
	end
end

pascal(5)
puts("*********************")
pascal(10)
puts("*********************")
pascal(1)
puts("*********************")
pascal(2)