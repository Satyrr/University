def simpson n
	a,b = -1.0, 1.0
	h = 2.0/n
	sum1, sum2 = 0.0,0.0

	sum1 += Math.cos(a)/2.0
	sum1 += Math.cos(b)/2.0

	(n/2 - 1).times do |i|
		sum1 += Math.cos(5*(a+2.0*(i+1)*h)-1)
	end

	(n/2).times do |i|
		sum2 += Math.cos(5*(a+(2.0*i+1)*h)-1)
	end

	return (h/3.0)*(2*sum1 + 4*sum2)
end

params = ActionController::Parameters.new({
  person: {
    name: 'Francesco',
    age:  22,
    role: 'admin'
  }
})

permitted = params.require(:person).permit(:name, :age)
permitted            # => {"name"=>"Francesco", "age"=>22}
permitted.class      # => ActionController::Parameters
permitted.permitted? # => true

Person.first.update!(permitted)