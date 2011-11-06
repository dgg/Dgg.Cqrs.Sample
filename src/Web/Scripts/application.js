$(function () {
	$('a.post').click(function (event) {
		event.stopPropagation();
		var url = $(this).parent('form').attr('action');
		alert(url);
		$.post(url, null, function (data) { alert('solution assigned'); });
	});
});