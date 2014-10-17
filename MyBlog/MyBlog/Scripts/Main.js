$(document).ready(function () {
    $('.showComments').on('Click', function () {
        var parent = $(this).parent();
        var commentsToShowOrHide = parent.find('.commentsDiv');
        commentsToShowOrHide.slideToggle();
    });

    //handle the "like" button
    $('.like').on('click', function () {
        //what post does the user want to like?
        var postID = $(this).data('postID');
        //store the 'this' element into a variable
        var theLikeButton = $(this);
        //do the AJAX get request to like the post
        $.get('/Home/LikePost' + postID, function (data) {
            //update the HTML with the current number of likes 
            theLikeButton.parent.html(data);
        });
    });

    //wire up the AJAX post for the comment form
    $('commentsDiv').on('Submit', function () {
        event.preventDefault();
        var theForm = $(this);
        $.post(theForm.attr('ation'), theForm.serialize(), function (data) {
            //update the HTML
            theForm.before(data);
            theForm.find('.commentAuthor', '.commentBody').val('');
        });
    });
});