/*document.addEventListener < Element by id = "rulesCbx" > {
    if(rulesCbx == false) {
    Response.Write("<script>alert('Please agree to the rules of the company')</script> ");
}
}*/

const checkbox = document.getElementById('rulesCbx');
const al = document.getElementById('like');
var button = document.getElementById('createbtn')


button.addEventListener('click', e => {

    if (!checkbox.checked) {
        alert('Please agree to the rules of the company');

    }
    else {
     document.getElementById('create').submit();
    }

});
al.onclick('click', e => {
    if (al.click) {
        history.go(-1);
    }
})

