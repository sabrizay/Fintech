// Varyant ekleme
$('.add-product-variant-btn').on('click', event => {
	const variantsBlock = $('.product-variants');
	let lastVariant = variantsBlock.find('.product-variant').last();
	let variantCount = parseInt(lastVariant.find('.variant-number').text());

	variantsBlock.find('select').select2('destroy');

	let lastVariantClone = lastVariant.clone();

	lastVariantClone.find('input').val('');

	lastVariantClone.find('.variant-number').text(++variantCount);

	lastVariantClone = changeProductSingleVariantAttr(lastVariantClone, variantCount);

	lastVariantClone.appendTo(variantsBlock);

	variantsBlock.find('select').select2();
});

// Varyant silme
const deleteSingleVariant = (elem) => {
	let deletedVariantNumber = elem.getAttribute('data-target-id');
	let variantCount = $('.product-variant').length;

	if (deletedVariantNumber == 1) {
		Swal.fire({
			title: 'Uyarı!',
			text: 'Tanımlanan ilk varyant silinemez!',
			icon: 'warning',
			confirmButtonText: 'Tamam'
		})
	}
	else {
		elem.closest('.product-variant').remove();

		if (deletedVariantNumber == variantCount)
			return;

		const variantsBlock = $('.product-variants');

		variantsBlock.find('select').select2('destroy');

		let productVariants = variantsBlock.find('.product-variant');

		productVariants.each((index, variant) => {
			if (index == 0)
				return;

			let variantNumberBlock = $(variant).find('.variant-number');
			let variantNumber = variantNumberBlock.text();

			variantNumberBlock.text(index + 1);

			changeProductSingleVariantAttr($(variant), index + 1);
		});

		variantsBlock.find('select').select2();

		Swal.fire({
			text: 'Varyant başarıyla silindi!',
			icon: 'success',
			timer: 1000,
			showConfirmButton: false
		});
	}
}


$(document).ready(function () {
	Dropzone.autoDiscover = false;
	$("#dZUpload").dropzone({
		url: "hn_SimpeFileUploader.ashx",
		addRemoveLinks: true,
		success: function (file, response) {
			var imgName = response;
			file.previewElement.classList.add("dz-success");
			console.log("Successfully uploaded :" + imgName);
		},
		error: function (file, response) {
			file.previewElement.classList.add("dz-error");
		}
	});
});

// Varyant attribute değiştirme
const changeProductSingleVariantAttr = (variant, variantNumber) => {
	$(variant).find("[id^='vertical-variant']").attr({
		id: `vertical-variant-${variantNumber}`,
		name: `vertical-variant-${variantNumber}`
	});

	$(variant).find("[for^='vertical-']").each((i, elem) => {
		let elemForAttr = elem.getAttribute('for');

		elem.setAttribute('for',
			variantNumber == 2
				? `${elemForAttr}-${variantNumber}`
				: `${elemForAttr.substring(0, elemForAttr.length - 2)}-${variantNumber}`
		);
	});

	$(variant).find(".product-variant-values [id^='vertical-']")
		.each((i, elem) => {
			let elemIdAttr = elem.getAttribute('id');

			elem.setAttribute('id',
				variantNumber == 2
					? `${elemIdAttr}-${variantNumber}`
					: `${elemIdAttr.substring(0, elemIdAttr.length - 2)}-${variantNumber}`
			);

			let elemNameAttr = $(elem).attr('name');

			if (elemNameAttr)
				elem.setAttribute('name',
					variantNumber == 2
						? `${elemNameAttr}-${variantNumber}`
						: `${elemNameAttr.substring(0, elemNameAttr.length - 2)}-${variantNumber}`
				);
		});

	$(variant).find('.product-variant-remove-btn')
		.attr('data-target-id', () => variantNumber);

	return variant;
}

$('#dpz-upload-image').dropzone({
	paramName: 'file',
	maxFile: 3,
	maxFilesize: 10, // MB
	acceptedFiles: 'image/jpg,image/jpeg,image/png',
	clickable: true,
	addRemoveLinks: true,
	dictRemoveFile: '<svg xmlns="http://www.w3.org/2000/svg" width="30" height="30" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather-trash-2"><polyline points="3 6 5 6 21 6"></polyline><path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"></path><line x1="10" y1="11" x2="10" y2="17"></line><line x1="14" y1="11" x2="14" y2="17"></line></svg>',
	thumbnailHeight: 150,
	thumbnailMethod: 'crop',

	init: function () {
		this.on("addedfile", function (file) {
			let addedImage = file.previewElement;

			let tmp = $('<div></div>').addClass('col-6 col-md-3 col-xl-2 draggable').html(addedImage);

			$('.uploaded-images #card-drag-area').append(tmp);
		});

		this.on('removedfile', function (file) {
			$('.uploaded-images #card-drag-area .draggable').each((i, val) => {
				if (val.innerHTML === '')
					val.remove();
			});
		});
	}
});

// Quill Editor Font Size
var Size = Quill.import('attributors/style/size');
Size.whitelist = ['8px', '9px', '10px', '12px', '14px', '16px', '20px', '24px', '32px', '42px', '54px', '68px', '84px', '98px'];
Quill.register(Size, true);

// Quill Editor Toolbar
var toolbarOptions = [
	[{ 'header': [1, 2, 3, 4, 5, 6, false] }],
	[{ 'size': Size.whitelist }],
	['bold', 'italic', 'underline', 'strike', 'link'],

	[{ 'script': 'super' }],
	[{ 'list': 'bullet' }, { 'list': 'ordered' }],
	[{ 'align': [] }, { 'indent': '-1' }, { 'indent': '+1' }, { 'direction': 'rtl' }],

	[{ 'color': [] }, { 'background': [] }],
	['image'],
	['code-block'],
	['showHtml'],

	['clean']
];

// Qill Editor Initialize
var quill = new Quill('#product-description-editor', {
	modules: {
		toolbar: toolbarOptions
	},
	theme: 'snow'
});

// showHtml Quill Editor
$('.ql-showHtml').on('click', () => {
	let quillHtmlEditor = $('.ql-html-editor')[0];

	if (quillHtmlEditor) {
		let quillHtmlContent = $('.ql-html-editor').text();

		quill.container.removeChild(quillHtmlEditor);

		$('.ql-editor').removeClass('d-none').html(quillHtmlContent);
	} else {
		let quillEditor = $('.ql-editor').first();

		let quillEditorClone = quillEditor.clone(true);
		quillEditorClone.addClass('ql-html-editor')
			.text(quill.root.innerHTML);

		quillEditor.addClass('d-none');

		quillEditorClone.appendTo('.ql-container');
	}
});