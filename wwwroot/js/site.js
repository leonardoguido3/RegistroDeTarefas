
//Quando ele chamar o alert, vai esconder automaticamente em 2,5s
$('.close-alert').ready(function () {
    setTimeout(function () {
        $('.alert').hide('hide');
    }, 4000);
    setTimeout(function () {
        $(".preloading").hide();
    }, 500);

    $('.cnpj').mask('00.000.000/0000-00', { reverse: true });
});
//Quando ele chamar o alert, vai esconder caso apertar o botao de fechar
$('.close-button').click(function () {
    $('.alert').hide('hide')
})

$(document).ready(function () {
    getDatatable('#tabelaEmpresa');
    getDatatable('#tabelaUsuario');
    getDatatable('#tabelaTarefa');
    getDatatable('#tabelaDashboard');
});

//Funcao para chamar paginacao BR do Datatables
function getDatatable(id) {
    $(document).ready(function () {
        $(id).DataTable({
            language: {
                url: '/dist/datatables/i18n.json'
            }
        });

    });
}

function BaixarRelatorio() {
    fetch('/Tarefa/relatorio')
        .then(resp => resp.blob())
        .then(blob => {
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.style.display = 'none';
            a.href = url;
            // the filename you want
            a.download = 'relatorio-tarefas.csv';
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
        });
}
