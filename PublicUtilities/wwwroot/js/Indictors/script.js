function updateResult(name) {
    var num1 = parseFloat(document.getElementById('num1_' + name).value) || 0;
    var num2 = parseFloat(document.getElementById('num2_' + name).value) || 0;

    var result = num1 - num2;

    document.getElementById('result_' + name).innerText = result;
}

function isNumberKey(evt) {
  var charCode = (evt.which) ? evt.which : evt.keyCode
  if (charCode > 31 && (charCode < 48 || charCode > 57))
    return false;
  return true;
}