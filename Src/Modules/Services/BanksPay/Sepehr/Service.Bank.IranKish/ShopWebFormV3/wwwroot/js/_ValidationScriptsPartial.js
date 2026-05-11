<script type="text/javascript">
    function submitGenerateTokenForm() {
            var data = $("#getTokenForm").serialize();
    console.log(data);
    alert(data);
    $.ajax({
        type: 'POST',
    url: '/Home/getToken_onclick',
    contentType: 'application/x-www-form-urlencoded; charset=UTF-8', // when we use .serialize() this generates the data in query string format. this needs the default contentType (default content type is: contentType: 'application/x-www-form-urlencoded; charset=UTF-8') so it is optional, you can remove it
    data: data,
    success: function (result) {
        alert('Successfully received Data ');
    console.log(result);
                },
    error: function () {
        alert('Failed to receive the Data');
    console.log('Failed ');
                }
            })
        }

</script>