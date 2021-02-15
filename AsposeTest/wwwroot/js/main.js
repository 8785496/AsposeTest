$(document).ready(() => {
    let fileName;

    $('#open').click(() => {
        $('#file').trigger('click');
    });

    $('#file').on('change', e => {
        $('#loader').removeClass('hidden');

        const formData = new FormData();
        formData.append('file', e.target.files[0], e.target.fileName);
        $.ajax({
            url: '/image/upload',
            type: 'post',
            processData: false,
            contentType: false,
            data: formData,
            success: function (data) {
                console.log('data', data);
                fileName = data.name;
                $('#preview').attr('src', `/image/preview?fileName=${fileName}`);
            }
        });
    });

    $('#download').click(() => {
        const radius = Number($('#radius').val());
        const fileType = $('#fileType').val();
        if (fileName && fileType) {
            const time = new Date().getTime();
            const url = `/image/gaussianBlur?fileType=${fileType}&fileName=${fileName}&radius=${radius}&_${time}`;
            download(url, 'output');
        }
    });

    $('#radius').on('input', e => {
        const radius = e.target.value;
        $('#preview').css('filter', `blur(${radius}px)`);
    });

    $('#preview').on('load', e => {
        console.log(e.target)
        $(e.target).removeClass('hidden');
        $('#loader').addClass('hidden');
    })

    function download(url, filename) {
        var a = document.createElement("a");
        a.href = url;
        a.setAttribute("download", filename);
        a.click();
    }
});
