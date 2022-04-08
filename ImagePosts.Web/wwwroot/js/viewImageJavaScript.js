$(() => {
    const id = $("#image-id").val();


    $("#like-button").on('click', function () {
     
        $.post('/home/likeit', { id }, () => {
            $("#like-button").attr('disabled', 'true');
        });
            
    });

       setInterval(() => {
        $.get('/home/getlikes', { id }, function (num) {
            $("#likes-count").text(num);
        });

    }, 500);
   


   
});