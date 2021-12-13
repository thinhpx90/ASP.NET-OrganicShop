//get_viewed_items_html...  
function get_viewed_items_html($current_product)
{
	// saving current viewed-item 
	var jsonProducts = sessionStorage.getItem('products_viewed'); 
	var arrPro = {}; 
	if( jsonProducts != null  ) 
		arrPro = JSON.parse( jsonProducts );  
	else
	{
		sessionStorage.removeItem('products_viewed'); 
		sessionStorage.removeItem('products_viewed_indexing'); // must-have this LOC 
	}

	// var $current_product = ; // $current_product object, ko phải string...   
	if($current_product != null && arrPro[$current_product.id] == null) // null / undefined 
	{ 
		arrPro[$current_product.id] = $current_product;   
		sessionStorage.setItem('products_viewed', JSON.stringify( arrPro ));  // 


		// saving current index 
		var jsonProIndex = sessionStorage.getItem('products_viewed_indexing'); 

		var arrProIndex = []; 
		if( jsonProIndex != null )  
			arrProIndex = JSON.parse( jsonProIndex );  
		arrProIndex.unshift($current_product.id);  
		sessionStorage.setItem('products_viewed_indexing', JSON.stringify( arrProIndex ));  // 

	}


	var jsonProIndex = sessionStorage.getItem('products_viewed_indexing'); 
	var jsonProducts = sessionStorage.getItem('products_viewed'); 
	var arrProIndex = []; 
	var $strHTML = ''; 
	var $countViewedItem = 0; 
	var $intMaxViewedItems = ''; 
	if($intMaxViewedItems == '')
		$intMaxViewedItems = 3; 
	else 
		$intMaxViewedItems = parseInt($intMaxViewedItems); 
	if(jsonProIndex != null & jsonProducts != null & $current_product != null )
	{
		//parse indexing, products...  
		arrProIndex = JSON.parse(jsonProIndex);   
		arrPro = JSON.parse( jsonProducts );   

		// assign count_items = 0;
		for (i=0; i<arrProIndex.length; i++ )
		{

			$strProID = arrProIndex[i];  
			if( $current_product.id != $strProID && $strProID != null && $countViewedItem < $intMaxViewedItems)
			{ 

				var product_viewed = arrPro[$strProID];
				//console.log(product_viewed);
				var price = Haravan.formatMoney(product_viewed.price, "") + '' + ' đ</b>'; 
				var compare_price = Haravan.formatMoney(product_viewed.compare_at_price, "") + '' + ' đ</b>'; 
				var old_price = '';
				if (product_viewed.price < product_viewed.compare_at_price) {
					old_price = '<del>'+compare_price+'</del>';
				}
				// for (img_idx ; i<product_viewed.images.length; img_idx++) 

				$bo_found = true;  
				//product_viewed['images'][0]; 
				//==JSON.parse(localStorage.getItem('products_viewed'))['1000302443']['images'][0]		
				$strHTML += '<div class="spost clearfix"> <div class="entry-image">'
				+'<a href="'+ product_viewed.url + '" title="'+ product_viewed.title + '">'  
				+' <img' 
				+ ' u="image" '
				+' src="'+ product_viewed['images'][0] + '"'
				+' alt="'+ product_viewed.title + '"'
				+ ' data-big="'+ product_viewed['images'][0] + '"'
				+' data-title="'+ product_viewed.title + '"'
				+' data-description="'+ product_viewed.title + '"'
				+'/>' 
				+'</a>'
				+'</div>'
				+'<div class="entry-c">'
				+'<div class="entry-title">'
				+'<h4><a href="'+ product_viewed.url +'">'+ product_viewed.title +'</a></h4>'
				+'</div>'
				+'<ul class="entry-meta"><li class="color">'+ old_price +'<ins> '+price+'</ins></li></ul>'
				+'</div></div>'

				$countViewedItem = $countViewedItem + 1; 

			} //  
		} // endfor: arrProIndex   
	} // endif: jsonProIndex

	return $strHTML;
}// get_vied_items_html


// <<<<<< product BEGIN  
function refreshProductSelections($tagSelectOption0, $option0, $tagSelectOption1 , $option1, $tagSelectOption2, $option2) 
{
	if($option0 != null && $option0 != '')
	{ 	
		//change option 0  
		$($tagSelectOption0 + ' option[value="'+$option0+'"]').prop('selected', true); // option-0 => Shape...  okok 
		$($tagSelectOption0).change(); 
	}


	if($option1 != null && $option1 != '')
	{ 
		//change option 1  
		$($tagSelectOption1 + ' option[value="'+$option1+'"]').prop('selected', true); // option-1 => Color...  okok 
		$($tagSelectOption1).change();  
	}
	if($option2 != null && $option2 != '')
	{ 
		//change option 2
		$($tagSelectOption2 + ' option[value="'+$option2+'"]').prop('selected', true); // option-1 => Color...  okok 
		$($tagSelectOption2).change();  
	}
}

function update_variant(variant, $tagPrice, $tagPriceCompare, $tagAddToCart, $tagProductSection) 
{
	var $unit_price = 0; 
	var $unit_price_compare = 0; 
	if(variant != null && variant.available==true )
	{ 
		$unit_price = variant.price;
		if(variant.price < variant.compare_at_price){
			$unit_price_compare = variant.compare_at_price;  

			//show onsale label
			$($tagProductSection).find('.sticker-sale').removeClass('hidden');  
		} else{

			//hide onsale label... nono: find matching ids: ('[id^="ProductDetails"]')  
			$($tagProductSection).find('.sticker-sale').addClass('hidden');  
		}

		$($tagAddToCart).html('Thêm vào giỏ'); 
		$($tagAddToCart).removeAttr('disabled');  
	}   
	else{
		$unit_price = variant.price;
		var $strUnitPrice = Haravan.formatMoney($unit_price, "") + 'đ';  // ''  shop.money_format
		var $strUnitPriceCompare = Haravan.formatMoney($unit_price_compare, "") + 'đ';  // ''  shop.money_format
		$($tagPrice).html($strUnitPrice); 
		$($tagAddToCart).html('Hết hàng'); 
		$($tagAddToCart).prop('disabled', true); 
	}

	var $strUnitPrice = Haravan.formatMoney($unit_price, "") + 'đ';  // ''  shop.money_format
	var $strUnitPriceCompare = Haravan.formatMoney($unit_price_compare, "") + 'đ';  // ''  shop.money_format
	$($tagPrice).html($strUnitPrice); 
	console.log($strUnitPrice);
	if($unit_price_compare > 0)
	{
		$($tagPriceCompare).html($strUnitPriceCompare);   
	}
	else 
		$($tagPriceCompare).html('');   

	$($tagProductSection).find('.unit_price_not_formated').val($unit_price);    
	// update_total();
}

//ajax: add to cart 
function addItem(form_id) {
	$.ajax({
		type: 'POST',
		url: '/cart/add.js',
		dataType: 'json',
		data: $('#'+form_id).serialize(),
		success: function(data) {
			Haravan.onSuccess(data, '#'+form_id)
		},
		error: function(XMLHttpRequest, textStatus) {
			Haravan.onError(XMLHttpRequest, textStatus);
		}
	});
}

Haravan.onSuccess = function(data, form_id) {
	addToCartPopup(data);
	//update top cart: qty, total price
	var $product_page = $(form_id).parents('[class^="product-page"]'); 
	var quantity = parseInt($product_page.find('[name="quantity"]').val(), 10) || 1;
	var $item_qty_new = 0; 
	var $item_price_new = 0; 
	var $item_price_increase = 0; 
	var $boUpdated = false; 

	//insert "no_item" html  
	if($('.top-cart-block .top-cart-content .top-cart-item').size() <= 0) 
	{
		$('.top-cart-block .top-cart-content').html(top_cart_no_item);  
	} 
	//update items 
	$('.top-cart-block .top-cart-content .top-cart-item').each(function(){	
		if($(this).find('.item_id').val() == $product_page.find('[name="id"]').val() ){
			$item_qty_new = parseInt($(this).find('.item_qty').val()) + quantity ;
			$item_price_single = parseFloat($(this).find('.item_unit_price_not_formated').val());
			$item_price_new = $item_qty_new * $item_price_single;   

			$item_price_increase = quantity * parseFloat($(this).find('.item_unit_price_not_formated').val());   
			$(this).find('.item_qty').val($item_qty_new);  // !!!
			$(this).find('.top-cart-item-quantity').html('x ' + $item_qty_new); 
			$(this).find('.top-cart-item-price').html(Haravan.formatMoney($item_price_new, "") + 'đ');  // ''  shop.money_format
			$boUpdated = true; // updated item 
		} 
	});

	if($boUpdated == false){ // current item is not existed!!!  
		var $proURL = $product_page.find('.product_url').val();
		var $proTitle = $product_page.find('.product_title_hd').val();
		var $proUnitPrice = parseFloat($product_page.find('.unit_price_not_formated').val());
		var $strNewItem = '<div class="top-cart-item clearfix">'
		+ ' <input type="hidden" class="item_id" value="'+ $product_page.find('[name="id"]').val() +'"></input>'  
		+ ' <input type="hidden" class="item_qty" value="'+ quantity +'"></input>' 
		+ ' <input type="hidden" class="item_unit_price_not_formated" value="'+ $proUnitPrice +'"></input>' 

		+ '<div class="top-cart-item-image">'
		+ ' <a href="'+ $proURL +'"><img src="'+ $product_page.find('.product_img_small').val() +'" alt="'+ $proTitle +'" ></a>'
		+ '</div>'
		+ '<div class="top-cart-item-desc">'
		//+ ' <span class="cart-content-count">x'+ quantity +'</span>'
		+ '<a href="'+ $proURL +'">' + $proTitle + '</a>'
		+ '<span class="top-cart-item-price">'+ Haravan.formatMoney($proUnitPrice * quantity, "") + 'đ' +'</span>' 
		+ '<span class="top-cart-item-quantity">x '+ quantity +'</span>'
		+'<a class="top_cart_item_remove" onclick = "deleteCart('+ $product_page.find('[name="id"]').val() +');"><i class="fa fa-times-circle"></i></a>'
		+ ' </div>'
		+ '</div>';
		$('.top-cart-block .top-cart-content .top-cart-items').append($strNewItem); 
		$item_price_increase = $proUnitPrice * quantity; 

	}  
	//check is emptiness...   
	check_topcart_empty();  

	//update total 
	var $quantity_new = parseInt($('.top-cart-block #top-cart-trigger span').text()) + quantity;  
	var $price_new = parseFloat($('.top-cart-block .top_cart_total_price_not_format').val()) + $item_price_increase;  
	$('.top-cart-block .top_cart_total_price_not_format').val($price_new);  // !!!
	// top cart total quantity
	$('.top-cart-block #top-cart-trigger span').html($quantity_new); 
	// scroll cart total quantity
	$('.scroll_cart span').html($quantity_new);
	$('.top-cart-block .top-checkout-price').html(Haravan.formatMoney($price_new, "") + 'đ'); 	

};


var top_cart_no_item = ''; 
function check_topcart_empty(){  

	//Bạn chưa mua sản phẩm nào! 
	if($('.top-cart-block .top-cart-content .top-cart-item').size() <= 0) 
	{		
		top_cart_no_item = $('.top-cart-block .top-cart-content').html();   
		$('.top-cart-block .top-cart-content').html();
	}
	else{
		//remove width, okok!!! 
		$('.top-cart-block .top-cart-content').css('width', '');
	}
}
jQuery(document).ready(function($){

	//select first size&color. 
	//second item: $($("#colorPicker option").get(1))...  
	$("#sizePicker option:first").attr('selected', 'selected'); 
	$("#colorPicker option:first").attr('selected', 'selected'); 

	// function: choose size  
	$('#option-0 select').change(function(){
		var $size = $(this).val(); 
		var $color = $('#option-1 select').val();
		var $material	= $('#option-2 select').val();
		var $tagSelectOption0 = '#product-select-option-0'; 
		var $tagSelectOption1 = '#product-select-option-1'; 
		var $tagSelectOption2 = '#product-select-option-2'; 

		refreshProductSelections($tagSelectOption0, $size, $tagSelectOption1 , $color,$tagSelectOption2 , $material);
	});

	// function: choose color  
	$('#option-1 select').change(function(){
		var $size = $('#option-0 select').val(); 
		var $color = $(this).val();
		var $material	= $('#option-2 select').val();  
		var $tagSelectOption0 = '#product-select-option-0'; 
		var $tagSelectOption1 = '#product-select-option-1'; 
		var $tagSelectOption2 = '#product-select-option-2'; 

		refreshProductSelections($tagSelectOption0, $size, $tagSelectOption1 , $color,$tagSelectOption2 , $material);
	});

	// function: choose material
	$('#option-2 select').change(function(){
		var $size = $('#option-0 select').val(); 
		var $color = $('#option-1 select').val();
		var $material = $(this).val();  
		var $tagSelectOption0 = '#product-select-option-0'; 
		var $tagSelectOption1 = '#product-select-option-1'; 
		var $tagSelectOption2 = '#product-select-option-2'; 

		refreshProductSelections($tagSelectOption0, $size, $tagSelectOption1 , $color,$tagSelectOption2 , $material);
	});

	$("#option-0 select option:first").attr('selected', 'selected'); 
	$("#option-1 select option:first").attr('selected', 'selected'); 
	$("#option-2 select option:first").attr('selected', 'selected'); 
	var $size = $("#option-0 select option:first").val(); 
	var $color = $("#option-1 select option:first").val();
	var $material	= $("#option-2 select option:first").val();
	var $tagSelectOption0 = '#product-select-option-0'; 
	var $tagSelectOption1 = '#product-select-option-1'; 
	var $tagSelectOption2 = '#product-select-option-2'; 

	refreshProductSelections($tagSelectOption0, $size, $tagSelectOption1 , $color,$tagSelectOption2 , $material);


	//add to cart 
	$(".add-to-cart").on('click', function(e) {  //.click(function(e){ // 
		e.preventDefault();
		addItem('ProductDetailsForm');
		console.log('addtocart');
	}); 

	//add to cart for QuickView
	$("#addtocartQV").on('click', function(e) {  //.click(function(e){ // 
		e.preventDefault();
		addItem('ProductDetailsFormQV');

	});

	//check empty for top-cart... 
	check_topcart_empty(); 

	//change qty... 
	$('.product-quantity input.quantity').on('change', function(){
		var $qty = parseInt($(this).val()); 
		if($qty <= 0){
			$(this).parents('[class^="product-page"]').find('[id^="addtocart"]').addClass('disabled'); 
		}
		else{
			$(this).parents('[class^="product-page"]').find('[id^="addtocart"]').removeClass('disabled'); 
		}
	});
	// buy now
	$('.buynow').on('click', function(e) {
		var form = $(this).closest('form').attr('id');
		e.preventDefault();
		buyNow(form);
	});
	// end buy now
	$('.scroll_buynow a').on('click', function(e) {
		e.preventDefault();
		$('.single-product .buynow').click();
	})
});  

// >>>>>> product END

// buy now 
function buyNow(form) {
	//var callback = function() {
	//		window.location = '/checkout';
	//	}
	var params = {
		type: 'POST',
		url: '/cart/add.js',
		data: jQuery('#'+form).serialize(),
		dataType: 'json',
		success: function() {
			//Haravan.updateCartNote('note', callback);
			window.location = '/checkout';
		},
		error: function(XMLHttpRequest, textStatus) {
			Haravan.onError(XMLHttpRequest, textStatus);
		}
	};
	jQuery.ajax(params);
}
// quick delete cart

function getCartAjax(){
	var cart = null;
	jQuery.getJSON('/cart.js', function(cart, textStatus) {
		if(cart)
		{
			var html = '';
			// update item for top cart
			$.each(cart.items,function(i,item) {
				html += '<div class="top-cart-item clearfix"> '
				+'<input type="hidden" class="item_id" value="'+ item.variant_id +'">'
				+'<input type="hidden" class="item_qty" value="'+ item.quantity +'">'
				+'<input type="hidden" class="item_unit_price_not_formated" value="' + item.price + '">'
				+'<div class="top-cart-item-image">'
				+'<a href="'+ item.url +'">'
				+'<img src="'+ Haravan.resizeImage(item.image,'small') +'" alt="' + item.product_title + '"></a>'
				+'</div>'
				+'<div class="top-cart-item-desc">'
				+'<a href="'+ item.url +'">' + item.product_title + '</a>';
				if ( typeof(formatMoney) != 'undefined' ){
					html += '<span class="top-cart-item-price">' + Haravan.formatMoney(cart.total_price, formatMoney) + '</span>';
				}
				else {
					html += '<span class="top-cart-item-price">' + Haravan.formatMoney(cart.total_price, "") + ' đ' + '</span>';
				}
				html +='<span class="top-cart-item-quantity">x '+ item.quantity +'</span>'
				+'<a class="top_cart_item_remove" onclick = "deleteCart('+ item.variant_id +');"><i class="fa fa-times-circle"></i></a>'
				+'</div></div>';
			});
			if(cart.item_count > 0){
				$('.top-cart-items').html(html);
				$('.top-cart-block #top-cart-trigger span').html(cart.item_count);
				$('.top-cart-block .top-checkout-price').html(Haravan.formatMoney(cart.total_price, "") + 'đ'); 	
			}
			else {
				$('.top-cart-block #top-cart-trigger span').html(cart.item_count);
				$('.top-cart-block .top-cart-content').html(top_cart_empty); 
				$('.top-cart-block .top-cart-content').css('width', '220px'); 
			}
		}
		else {
			$('.top-cart-block .top-cart-content').html(top_cart_empty);
			$('.top-cart-block .top-cart-content').html(top_cart_empty); 
			$('.top-cart-block .top-cart-content').css('width', '200px'); 
		}
	});

}

// delete cart
function deleteCart(variant_id) {
	var params = {
		type: 'POST',
		url: '/cart/change.js',
		data: 'quantity=0&id=' + variant_id,
		dataType: 'json',
		success: function(cart) {
			getCartAjax();
		},
		error: function(XMLHttpRequest, textStatus) {
			Haravan.onError(XMLHttpRequest, textStatus);
		}
	};
	jQuery.ajax(params);
}

// add to cart popup
/**
 * Popup notify add-to-cart
 */
function notifyProduct($info){
	var wait = setTimeout(function(){
		$.jGrowl($info,{
			life: 5000
		});	
	});
}

function addToCartPopup(jqXHR, textStatus, errorThrown) {
	$.ajax({
		type: 'GET',
		url: '/cart.js',
		async: false,
		cache: false,
		dataType: 'json',
		//success: updateCartDesc
	});

	var $info = '<div class="row"><div class="col-md-4"><a href="'+ jqXHR['url'] +'"><img width="70px" src="'+ Haravan.resizeImage(jqXHR['image'], 'small') +'" alt="'+ jqXHR['title'] +'"/></a></div><div class="col-md-8"><div class="jGrowl-note"><a class="jGrowl-title" href="'+ jqXHR['url'] +'">'+ jqXHR['title'] +'</a><ins>'+ Haravan.formatMoney(jqXHR['price'], "") + '' + ' đ' +'</ins></div></div></div>';
	notifyProduct($info);
}


$(document).ready(function(){
	$('#searchauto').submit(function(e){
		e.preventDefault();
		window.location = '/search?q=filter=('+'(collectionid:product>=0)'+'&&(title:product**' + $(this).find('input[name="q"]').val() + ')||(sku:product=' + $(this).find('input[name="q"]').val() + '))';
	})
})















