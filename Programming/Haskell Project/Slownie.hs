module Slownie (Rodzaj(..), Waluta(..), slownie) where

data Rodzaj = Meski | Zenski | Nijaki deriving Show
data Waluta = Waluta {
    mianownik_poj :: String,
    mianownik_mn :: String,
    dopelniacz_mn :: String,
    rodzaj :: Rodzaj
} deriving Show

data Tysiac = Tysiac {
    mianownik_poj1 :: String,
    mianownik_mn1 :: String,
    dopelniacz_mn1 :: String
} deriving Show

slownie :: Waluta -> Integer -> String
slownie waluta liczba 
    | liczba > 10^6000 - 1 = "mnóstwo"
    | otherwise =  (if liczba < 0 then "minus " else "") ++ (kwotaSlownie rdz kwota 0 ) ++ walutaSlownie waluta kwota
       where rdz = rodzaj waluta
             kwota = bezwzgledna liczba


kwotaSlownie :: Rodzaj -> Integer -> Integer -> String -- kwotaSlownie rodzaj_waluty kwota akumulator_1000^acc
kwotaSlownie _ 0 0 = "zero "
kwotaSlownie _ 0 _ = ""
kwotaSlownie Nijaki 1 0 = "jedno "
kwotaSlownie Zenski 1 0 = "jedna " 
kwotaSlownie Zenski liczba 0 
	| (liczba `mod` 10 == 2) && (liczba `mod` 100 /= 12) = reszta ++ ponizejTysiacaSlownie bezCyfry ++ "dwie "
    where reszta = kwotaSlownie Zenski (liczba `div` 1000) 1
          bezCyfry = (liczba - (liczba `mod` 10)) `mod` 1000
kwotaSlownie waluta liczba mnoznik
	| ((liczba `mod` 1000) == 1) && mnoznik > 0 = reszta ++ (mnoznikSlownie liczba mnoznik) -- brak 'jeden' przed 'tysiac', 'milion' itd., 
	| otherwise = reszta ++ (ponizejTysiacaSlownie ponizejTysiaca) ++ (mnoznikSlownie liczba mnoznik)
    where reszta = kwotaSlownie waluta (liczba `div` 1000) (mnoznik+1)
          ponizejTysiaca = liczba `mod` 1000



ponizejTysiacaSlownie :: Integer -> String
ponizejTysiacaSlownie pelnaKwota
	| d/=1 = (reprezentacjaSlowna s setki) ++  (reprezentacjaSlowna d dziesiatki) ++ (reprezentacjaSlowna c cyfry)
	| d==1 = (reprezentacjaSlowna s setki) ++ (reprezentacjaSlowna c nascie)
    where (s,d,c) = cyfryLiczby (bezwzgledna pelnaKwota)

reprezentacjaSlowna :: Integer -> [(Integer,String)] -> String
reprezentacjaSlowna liczba ((cyfra,slowo):reszta)
    | cyfra == liczba = slowo
    | otherwise = reprezentacjaSlowna liczba reszta

setki :: [(Integer,String)]
setki = zip [0..9] ["" ,"sto ", "dwieście ", "trzysta ", "czterysta ", "pięćset ", "sześcset ", "siedemset ", "osiemset ", "dziewięćset "]
nascie :: [(Integer,String)]
nascie = zip [0..9] ["dziesięć ", "jedenaście ", "dwanaście ", "trzynaście ", "czternaście ", "piętnaście ", "szesnaście ", "siedemnaście ", "osiemnaście ", "dziewiętnaście "]
dziesiatki :: [(Integer,String)]
dziesiatki = zip [0..9] ["", "", "dwadzieścia ", "trzydzieści ", "czterdzieści ", "pięćdziesiąt ", "sześćdziesiąt ", "siedemdziesiąt ", "osiemdziesiąt ","dziewięćdziesiąt "]
cyfry :: [(Integer,String)]
cyfry = zip [0..9] ["", "jeden ", "dwa ", "trzy ", "cztery ", "pięć ", "sześć ", "siedem ", "osiem ", "dziewięć "]
	

cyfryLiczby :: Integer -> (Integer, Integer, Integer)
cyfryLiczby liczba = (setki, dzies, cyf)
	where setki = liczba `div` 100
	      dzies = (liczba `div` 10) `mod` 10
	      cyf   = liczba `mod` 10

mnoznikSlownie :: Integer -> Integer -> String -- tysiac, milion, miliard...
mnoznikSlownie liczba mnoznik 
	| kwota == 0 = ""
    | kwota == 1 = mianownik_poj1 tysiace
    | (kwota `mod` 100) `elem` [12,13,14] = dopelniacz_mn1 tysiace
    | (kwota `mod` 10) `elem` [2,3,4] = mianownik_mn1 tysiace
    | otherwise = dopelniacz_mn1 tysiace
    where kwota = (bezwzgledna liczba) `mod` 1000
          tysiace = okreslMnoznik mnoznik


okreslMnoznik :: Integer -> Tysiac
okreslMnoznik k 
	| k == 0 = Tysiac "" "" ""
	| k == 1 = Tysiac "tysiąc " "tysiące " "tysięcy "
    | k `mod` 2 == 0 = Tysiac (pk ++ "lion ") (pk ++ "liony ") (pk ++ "lionów ")
    | k `mod` 2 == 1 = Tysiac (pk ++ "liard ") (pk ++ "liardy ") (pk ++ "liardów ")
    where pk = przedrostek (k `div` 2)  

przedrostki1 :: [(Integer,String)]
przedrostki1 = zip [0..9] ["" ,"mi", "bi", "try", "kwadry", "kwinty", "seksty", "septy", "okty", "noni"]
przedrostki2 :: [(Integer,String)]
przedrostki2 = zip [0..9] ["", "un", "do", "tri", "kwatuor", "kwin", "seks", "septen", "okto", "nowem"]
przedrostki3 :: [(Integer,String)]
przedrostki3 = zip [0..9] ["", "decy", "wicy", "trycy", "kwadragi", "kwintagi", "seksginty", "septagi", "oktagi", "nonagi"]
przedrostki4 :: [(Integer,String)]
przedrostki4 = zip [0..9] ["", "centy", "ducenty", "trycenty", "kwadryge", "kwinge", "sescenty", "septynge", "oktynge", "nonge"]

przedrostek :: Integer -> String
przedrostek k 
	| k<10 = reprezentacjaSlowna k przedrostki1 
	| otherwise = (reprezentacjaSlowna a przedrostki2) ++ (reprezentacjaSlowna b przedrostki3) ++ (reprezentacjaSlowna c przedrostki4)      
	where a = k `mod` 10
	      b = (k `mod` 100) `div` 10
	      c = k `div` 100                      

--------- Waluta ---------

walutaSlownie :: Waluta -> Integer -> String -- wyznaczenie słownej reprezentacji waluty dla podanej kwoty
walutaSlownie waluta liczba 
    | kwota == 1 = mianownik_poj waluta
    | (kwota `mod` 100) `elem` [12,13,14] = dopelniacz_mn waluta
    | (kwota `mod` 10) `elem` [2,3,4] = mianownik_mn waluta
    | otherwise = dopelniacz_mn waluta
    where kwota = bezwzgledna liczba

-------- Inne --------
bezwzgledna :: Integer -> Integer
bezwzgledna x = if x<0 then -x else x
