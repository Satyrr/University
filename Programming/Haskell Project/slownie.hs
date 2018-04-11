import Slownie
import System.Environment


main = do 
  (arg1:arg2:rest) <- getArgs
  putStrLn $ slownie (okreslWalute arg2) (read arg1)

okreslWalute :: String -> Waluta
okreslWalute w = case w of "AUD" -> Waluta "dolar australijski" "dolary australijskie" "dolarów australijskich" Meski 
                           "BGN" -> Waluta "lew bułgarski" "lewy bułgarskie" "lewów bułgarskich" Meski
                           "BRL" -> Waluta "real brazylijski" "reale brazylijskie" "reali brazylijskich" Meski
                           "BYR" -> Waluta "białoruski rubel" "białoruskie ruble" "białoruskich rubli" Meski
                           "CAD" -> Waluta "dolar kanadyjski" "dolary kanadyjskie" "dolarów kanadyjskich" Meski
                           "CHF" -> Waluta "frank szwajcarski" "franki szwajcarskie" "franków szwajcarskich" Meski
                           "CNY" -> Waluta "yuan chiński" "yuany chińskie" "yuanów chińskich" Meski
                           "CZK" -> Waluta "korona czeska" "korony czeskie" "koron czeskich" Zenski
                           "DKK" -> Waluta "korona duńska" "korony duńskie" "koron duńskich" Zenski
                           "EUR" -> Waluta "euro" "euro" "euro" Nijaki
                           "GBP" -> Waluta "funt szterling" "funty szterlingi" "funtów szterlingów" Meski
                           "HKD" -> Waluta "dolar hongkoński" "dolary hongkońskie" "dolarów hongkońskich" Meski
                           "HRK" -> Waluta "kuna chorwacka" "kuny chorawckie" "kun chorwackich" Zenski
                           "HUF" -> Waluta "forint węgierski" "forinty węgierskie" "forintów węgierskich" Meski
                           "IDR" -> Waluta "rupia indonezyjska" "rupie indonezyjskie" "rupii indonezyjskich" Zenski
                           "ISK" -> Waluta "korona islandzka" "korony islandzkie" "koron islandzkich" Zenski
                           "JPY" -> Waluta "jen japoński" "jeny japońskie" "jenów japońskich" Meski
                           "KRW" -> Waluta "won południowokoreański" "wony południowokoreańskie" "wonów południowokoreańskich" Meski
                           "MXN" -> Waluta "peso meksykańskie" "peso meksykańskie" "peso meksykańskich" Nijaki
                           "MYR" -> Waluta "ringgit malezyjski" "ringgity malezyjskie" "ringgitów malezyjskich" Meski
                           "NOK" -> Waluta "korona norweska" "korony norweskie" "koron norweskich" Zenski
                           "NZD" -> Waluta "dolar nowozelandzki" "dolary nowozelandzkie" "dolarów nowozelandzkich" Meski
                           "PHP" -> Waluta "peso filipińskie" "peso filipińskie" "peso filipińskich" Nijaki
                           "PLN" -> Waluta "złoty" "złote" "złotych" Meski
                           "RON" -> Waluta "nowa leja rumuńska" "nowe leje rumuńskie" "nowych leji rumuńskich" Zenski
                           "RUB" -> Waluta "rubel rosyjski" "ruble rosyjskie" "rubeli rosyjskich" Meski
                           "SDR" -> Waluta "specjalne prawo ciągnienia" "specjalne prawa ciągnienia" "specjalnych praw ciągnienia" Meski
                           "SEK" -> Waluta "korona szwedzka" "korony szwedzkie" "koron szwedzkich" Zenski
                           "SGD" -> Waluta "dolar singapurski" "dolary singapurskie" "dolarów singapurskich" Meski
                           "THB" -> Waluta "baht tajski" "bahty tajskie" "bahtów tajskich" Meski
                           "TRY" -> Waluta "nowa lira turecka" "nowe liry tureckie" "nowych lir tureckich" Zenski
                           "UAH" -> Waluta "hrywna ukraińska" "hrywny ukraińskie" "hrywn ukraińskich" Zenski
                           "USD" -> Waluta "dolar amerykański" "dolary amerykańskie" "dolarów amerykańskich" Meski
                           "ZAR" -> Waluta "rand południowoafrykański" "randy południowoafrykańskie" "randów południowoafrykańskich" Meski
