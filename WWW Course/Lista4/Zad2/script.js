function checkPatt(element, warningElementId, elementName, patt)
{
	if(element.value.search(patt) == -1 && element.value.length != 0)
	{
		document.getElementById(warningElementId).innerHTML = "Podany " + elementName + " jest nieprawidłowy"
		return false
	}
	else
	{
		document.getElementById(warningElementId).innerHTML = ""
		return true
	}
}

function checkMail()
{
	var patt = /^[a-zA-Z0-9.!#$%&’*+\/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/i

	return checkPatt(document.getElementById("email"), "emailWarning", "e-mail", patt)
		
}	

function checkAccountNr()
{
	var patt = /^\d{2}( \d{4}){6}$/i

	return checkPatt(document.getElementById("accountNr"), "accountNrWarning", "numer konta", patt)
}	

function checkPesel()
{
	var patt = /^\d{11}$/i

	return checkPatt(document.getElementById("pesel"), "peselWarning", "PESEL", patt)
}	

function checkBirthDate()
{
	var patt = /^([0-2]\d|[3][0-1])\/([0]\d|[1][0-2])\/\d{4}$/i

	return checkPatt(document.getElementById("birthDate"), "birthDateWarning", "data", patt)
}	

function checkForm(event)
{
	
	var result = checkMail() && checkPesel() && checkBirthDate() && checkBirthDate()

	if(result == false)
		event.preventDefault()
}	



document.getElementById("accountNr").addEventListener("focusout",checkAccountNr)
document.getElementById("pesel").addEventListener("focusout",checkPesel)
document.getElementById("birthDate").addEventListener("focusout",checkBirthDate)
document.getElementById("email").addEventListener("focusout",checkMail)

document.getElementById("personForm").addEventListener("submit",checkForm)


//22 1122 1122 1122 1122 1122 1122
//78945612321
//21/08/1995
//daf@goadf.pl