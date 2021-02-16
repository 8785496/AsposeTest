$(document).ready(() => {
    let fileName;

    disableTools(true);

    $('#open').click(() => {
        $('#file').trigger('click');
    });

    $('#file').on('change', e => {
        if (!e.target.files[0]) { return; }
        $('.title > span').text(e.target.files[0].name);

        $('.loader').removeClass('hidden');

        const formData = new FormData();
        formData.append('file', e.target.files[0]);
        $.ajax({
            url: '/image/upload',
            type: 'post',
            processData: false,
            contentType: false,
            data: formData,
            success: data => {
                console.log('data', data);
                fileName = data.name;
                $('#preview').attr('src', `/image/preview?fileName=${fileName}`);
            },
            error: () => {
                $('.loader').addClass('hidden');
                $.jGrowl("Image not uploaded", { header: 'Error' });
            }
        });

        $("#file").val(null);
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
        const radius = `${e.target.value}px`;
        $('#radius-label').text(radius);
        $('#preview').css('filter', `blur(${radius})`);
    });

    $('#preview').on('load', () => {
        $('.preview-container').removeClass('hidden');
        $('.title').removeClass('hidden');
        $('.loader').addClass('hidden');
        $('#open').addClass('hidden');
        disableTools(false);
    })

    $('#preview').on('error', e => {
        $('.loader').addClass('hidden');
        $.jGrowl("Image not loaded", { header: 'Error' });
    })

    $('#close').click(() => {
        $('.preview-container').addClass('hidden');
        $('.title').addClass('hidden');
        $('.loader').addClass('hidden');
        $('#open').removeClass('hidden');
        disableTools(true);
    })

    function download(url, filename) {
        var a = document.createElement("a");
        a.href = url;
        a.setAttribute("download", filename);
        a.click();
    }

    function disableTools(disabled) {
        if (disabled) {
            $('#radius').attr('disabled', 'disabled');
            $('#fileType').attr('disabled', 'disabled');
            $('#download').attr('disabled', 'disabled');
        } else {
            $('#radius').removeAttr('disabled');
            $('#fileType').removeAttr('disabled');
            $('#download').removeAttr('disabled');
        }
    }
});
