function fibrec(n)
{
    if(n == 1 || n == 2)
      return 1;

    return fibrec(n-1) + fibrec(n-2);
}

var fibrec_mem = (function()
{
  var memoized = {};
  return function f(n)
  {
    if(memoized[n])
      return memoized[n]
    var result;
    if(n == 1 || n == 2)
      result = 1;
    else
      result = f(n-1) + f(n-2);

    memoized[n] = result;
    return result;
  };
})();


console.log('n\t' + 'bez memoizacji(ms)\t' + 'z memoizacja(ms)')
for(var n = 30; n < 40; n++)
{
    var start = new Date();
    var res1 = fibrec(n)
    var rek_time = new Date() - start;

    start = new Date();
    var res2 = fibrec_mem(n)
    var memo_time = new Date() - start;
    
    console.log(n + '\t' +  rek_time + '\t\t\t' + memo_time)
    
}

