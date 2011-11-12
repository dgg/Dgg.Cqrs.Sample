$(function () {
	$('a.post').click(function (event) {
		event.stopPropagation();
		var form = $(this).parent('form'); 
		var url = form.attr('action');
		$.ajax({
			async: false,
			type: 'POST',
			url: url,
			data: form.serialize(),
			success: function () {
				document.location.href = document.location.href;
			}
		});
	});
});