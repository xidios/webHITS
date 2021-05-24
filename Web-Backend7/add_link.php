<?php
function add_photo($link, $user_id)
{
    $msg = "";

    // If upload button is clicked ...

    $render = "SELECT * FROM photos ORDER BY id DESC LIMIT 1";
    $id = mysqli_query($link, $render);


    $filename = $id->fetch_assoc()['id'] + 1 . $_FILES["photo"]["name"];
    $tempname = $_FILES["photo"]["tmp_name"];
    $folder = "image/" . $filename;



    // Get all the submitted data from the form

    $sql = "INSERT INTO photos (link,user_id) VALUES ('$filename','$user_id')";

    // Execute query
    //echo $link->insert_id;
    $result = mysqli_query($link, $sql);

    // Now let's move the uploaded image into the folder: image
    if ($result) {
        if (move_uploaded_file($tempname, $folder)) {
            $msg = "Image uploaded successfully";
            return true;
        } else {
            $msg = "Failed to upload image";
            return false;
        }
    }
}

function load_photo($link, $user_id)
{
    $query = mysqli_query($link, "SELECT * FROM users where id = $user_id");
    if ($query) {
        $row = $query->fetch_assoc()['avatar'];
        if(isset($row)){
            $image_query = mysqli_query($link, "SELECT link FROM photos where id=$row");
            return 'image/'.$image_query->fetch_assoc()['link'];
        }
    }
    
}
