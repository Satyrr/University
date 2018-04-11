function liczby()
{
    for(let i = 1; i < 100000; i++)
    {
        let k = i
        let sum = 0

        while(k > 0)
        {
            if(k%10 == 0 || i%(k%10) != 0)
            {
                break;
            }
            sum += k % 10
            k = Math.floor(k/10)
        }

        if( k == 0 && i % sum == 0)
            console.log(i + '\n')
    }
}
liczby()
