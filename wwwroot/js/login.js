//Quando ele chamar o alert, vai esconder automaticamente em 2,5s
$('.close-alert').ready(function () {
    setTimeout(function () {
        $('.alert').hide('hide');
    }, 3000);
    setTimeout(function () {
        $(".preloading").hide();
    }, 500);

   
});

$("#formularioLogin").submit(function (e) {
    e.preventDefault();

    var inputsFormulario = $("#formularioLogin").find(":input");

    var objeto = {
        email: inputsFormulario.eq(0).val(),
        senha: inputsFormulario.eq(1).val()
    };

    var json = JSON.stringify(objeto);

    $.ajax({
        url: "https://localhost:44380/Autorizacao",
        method: 'POST',
        data: json,
        contentType: 'application/json',
        dataType: 'json'
    }).done(function (resposta) {
        SalvarDadosLogin(resposta);
        window.location.href = "dashboard.html";
    }).fail(function (err, errr, errrr) {
        Swal.fire({
            position: 'center',
            icon: 'error',
            title: 'Oops...',
            text: 'O Usuário e senha estão inválidos, tente novamente!',
            showConfirmButton: false,
            timer: 1800
        });
    });

});

function SalvarDadosLogin(dadosToken) {
    localStorage.setItem('bearer', dadosToken.bearer);
    localStorage.setItem('nomeUsuario', dadosToken.nomeUsuario);
    localStorage.setItem('nivelAcesso', dadosToken.nivelAcesso);
}