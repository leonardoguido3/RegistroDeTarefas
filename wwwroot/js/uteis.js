// lendo o documento e inserindo as mascaras
$(document).ready(function () {
    setTimeout(function () {
        $(".preloading").hide();
    }, 500);

    $('.cnpj').mask('00.000.000/0000-00', { reverse: true });

    $.ajaxSetup({
        //headers: { 'Authorization': 'Bearer' + localStorage.getItem('bearer')},
        error: function (jqXHR, textStatus, errorThrown) {
            if (jqXHR.status == 400) {
                Swal.fire({
                    icon: 'warning',
                    title: 'Oops...',
                    text: jqXHR.response.Text
                });
            } else if (jqXHR.status == 0) {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'Os nossos servidores ou sua internet estão indisponíveis no momento, tente novamente mais tarde.'
                });
            } else if (jqXHR.status == 401) {
                Swal.fire({
                    icon: 'info',
                    title: 'Oops...',
                    text: 'A sua sessão expirou, faça o login novamente!'
                }).then((result) => {
                    window.location.href = '/index.html';
                });
            } else if (jqXHR.status == 403) {
                Swal.fire({
                    icon: 'warning',
                    title: 'Acesso negado!',
                    text: 'Você não tem permissão para acecssar essa funcionalidade!'
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Empresa cadastrada com sucesso!',
                    text: jqXHR.responseText
                });
            }
        };
    });
    success: function (jqXHR, textStatus, errorThrown) {
        if (jqXHR.status >= 200 && jqXHR <= 204) {
            Swal.fire({
                icon: 'success',
                title: 'Oops...',
                text: jqXHR.response.Text
            });
        }
    };
});

Swal.fire({
    position: 'center',
    icon: 'success',
    title: 'Empresa atualizada com sucesso!',
    showConfirmButton: false,
    timer: 1800
});


// formatação basica do telefone
// function FormatarTelefone(texto) {
//     if (texto == null) {
//         return "";
//     } else {
//         return texto.replace(/^(\d{2})(\d{5})(\d{4})/, "($1) $2-$3");
//     }
// }

// formatação básica do CPF
function FormataCNPJ(cnpj) {
    var cnpjFormatado = cnpj.replace(/^(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, "$1.$2.$3/$4-$5");
    return cnpjFormatado;
}

// Remover mascara CPF
function RemoveMascaraCNPJ(cnpj) {
    return cnpj.replace(/\./g, "").replace(/\-/g, "").replace(/\//g, "");
}

// remover mascara Telefone
// function RemoveMascaraTelefone(telefone) {
//     return telefone.replace(/\(/g, "").replace(/\)/g, "").replace(/\-/g, "").replace(/\ /g, "");
// }

// formatação basica da data
function FormatarData(dataString) {
    let data = moment(dataString).format('DD/MM/YYYY');
    return data;
}

var nivelAcesso = localStorage.getItem('nivelAcesso');