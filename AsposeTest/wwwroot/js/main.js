$(document).ready(() => {
    let fileName;

    $('#form').submit(e => {
        e.preventDefault();
        $.ajax({
            url: '/image/upload',
            type: 'post',
            processData: false,
            contentType: false,
            data: new FormData(e.target),
            success: function (data) {
                console.log('data', data);
                fileName = data.name;
                $('#preview').attr('src', `/image/preview?fileName=${fileName}`);
            }
        });
    });

    $('#apply').click(() => {
        if (fileName) {
            const time = new Date().getTime();
            $('#result').attr('src', `/image/gaussianBlurPreview?fileName=${fileName}&_${time}`);
        }
    });
});
