$( function() {
    var dialog, form, deleteDialog, rowToDelete,

      name = $( "#name" ),
      surname = $( "#surname" ),
      city = $( "#city" ),
      postcode = $( "#postcode" ),
      birthday = $( "#birthday" ),
      allFields = $( [] ).add( name ).add( surname ).add( city ).add( postcode ).add( birthday ),
      tips = $( ".validateTips" );
 
    function updateTips( t ) {
      tips
        .text( t )
        .addClass( "ui-state-highlight" );
      setTimeout(function() {
        tips.removeClass( "ui-state-highlight", 1500 );
      }, 500 );
    }
 
    function checkLength( o, n, min, max ) {
      if ( o.val().length > max || o.val().length < min ) {
        o.addClass( "ui-state-error" );
        updateTips( "Długość pola " + n + " musi być pomiędzy " +
          min + " i " + max + "." );
        return false;
      } else {
        return true;
      }
    }
 
    function checkRegexp( o, regexp, n ) {
      if ( !( regexp.test( o.val() ) ) ) {
        o.addClass( "ui-state-error" );
        updateTips( n );
        return false;
      } else {
        return true;
      }
    }
 
    function addUser() {
      var valid = true;
      allFields.removeClass( "ui-state-error" );
 
      valid = valid && checkLength( name, "Imię", 3, 25 );
      valid = valid && checkLength( surname, "Nazwisko", 3, 25 );
      valid = valid && checkLength( city, "Miasto", 6, 80 );
      valid = valid && checkLength( postcode, "Kod pocztowy", 5, 8 );
      valid = valid && checkLength( birthday, "Data urodzenia", 1, 30 );
 
      valid = valid && checkRegexp( name, /^[a-z]([0-9a-z_\s])+$/i, "Imię może składać się z a-z, 0-9, _, spacji i musi zaczynać się od litery." );
      valid = valid && checkRegexp( surname, /^[a-z]([0-9a-z_\s])+$/i, "Nazwisko może składać się z a-z, 0-9, _, spacji i musi zaczynać się od litery." );
      valid = valid && checkRegexp( city, /^[a-z]([0-9a-z_\s])+$/i, "Miasto może składać się z a-z, 0-9, _, spacji i musi zaczynać się od litery." );
      valid = valid && checkRegexp( postcode, /^\d\d-\d\d\d$/, "Kod pocztowy musi być w formacie cc-ccc" );
      
      if ( valid ) {
        $( "#users tbody" ).append( "<tr>" +
          "<td>" + name.val() + "</td>" +
          "<td>" + surname.val() + "</td>" +
          "<td>" + city.val() + "</td>" +
          "<td>" + postcode.val() + "</td>" +
          "<td>" + birthday.val() + "</td>" +
          "<td>" + '<a href="#" class="delete-entry">Usuń</a>' + "</td>" +
        "</tr>" );
        dialog.dialog( "close" );
      }
      return valid;
    }

    function deleteEntry()
    {
      rowToDelete = $(this).parent().parent()
      deleteDialog.dialog("open")
    }
 
    dialog = $( "#dialog-form" ).dialog({
      autoOpen: false,
      height: 400,
      width: 350,
      modal: true,
      buttons: {
        "Utwórz użytkownika": addUser,
        Cancel: function() {
          dialog.dialog( "close" );
        }
      },
      close: function() {
        form[ 0 ].reset();
        allFields.removeClass( "ui-state-error" );
      }
    });

    deleteDialog = $( "#dialog-confirm" ).dialog({
      autoOpen: false,
      resizable: false,
      height: "auto",
      width: 400,
      modal: true,
      buttons: {
        "Usuń": function() {
          rowToDelete.remove();
          $( this ).dialog( "close" );
        },
        Cancel: function() {
          $( this ).dialog( "close" );
        }
      }
    });
    
    birthday.datepicker({dateFormat: "dd/mm/yy"});


    form = dialog.find( "form" ).on( "submit", function( event ) {
      event.preventDefault();
      addUser();
    });
 
    $( "#create-user" ).button().on( "click", function() {
      dialog.dialog( "open" );
    });

    $("#users").on('click', '.delete-entry', deleteEntry)

  } );