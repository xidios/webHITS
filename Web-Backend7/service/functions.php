<?php
require('models/enum.php');
function checkRole($link)
{
    $bearer = apache_request_headers()['Authorization'];
    if ($bearer == "") {
        return "not_authorized";
    }
    $select = "SELECT user_id FROM `tokens` WHERE `tokens`.`token` = '$bearer'";
    $get_username = mysqli_query($link, $select);
    if ($get_username) {
        $user = mysqli_fetch_assoc($get_username)['user_id'];
        //echo $user;
        $role = mysqli_query(
            $link,
            "SELECT * FROM users 
        INNER JOIN roles on users.role = roles.id
        WHERE users.id = $user"
        );
        if ($role) {
            $user_role = mysqli_fetch_assoc($role)['role'];
            return $user_role;
        }
    }
}
function owner($link, $user_id)
{
    $bearer = apache_request_headers()['Authorization'];
    if ($bearer == "") {
        return "not_authorized";
    }
    $select = "SELECT token,user_id FROM tokens WHERE token = '$bearer' AND user_id=$user_id";
    $get_username = mysqli_query($link, $select);

    if ($get_username) {
        return "owner";
    }
}

function owner2($link)
{
    $bearer = apache_request_headers()['Authorization'];
    if ($bearer == "") {
        return "not_authorized";
    }
    $select = "SELECT token,user_id FROM tokens WHERE token = '$bearer'";
    $get_username = mysqli_query($link, $select);

    if ($get_username) {
        return "owner";
    }
}

function getFormData($method)
{
    // GET или POST: данные возвращаем как есть
    if ($method === 'GET') return $_GET;
    if ($method === 'POST') return $_POST;

    // PUT, PATCH или DELETE

    // $data = array();
    // $exploded = explode('&', file_get_contents('php://input'));
    $data = file_get_contents('php://input');
    $data = json_decode($data, true);
    // foreach ($exploded as $pair) {
    //     $item = explode('=', $pair);
    //     if (count($item) == 2) {
    //         $data[urldecode($item[0])] = urldecode($item[1]);
    //     }
    // }

    return $data;
}



function getAllUsers($link)
{
    $admin = checkRole($link);
    if ($admin == 'user' || $admin == 'not_authorized') {
        $users = mysqli_query($link, "SELECT active, cities.name FROM users 
    INNER JOIN cities on users.city = cities.id;");
    } else if ($admin == 'admin') {
        $users = mysqli_query($link, "SELECT active, birthday, cities.name, roles.role FROM users 
        INNER JOIN cities on users.city = cities.id
        INNER JOIN roles on users.role = roles.id;");
    }
    if ($users) {
        $usersList = [];
        while ($user = mysqli_fetch_assoc($users)) {
            $usersList[] = $user;
        }

        echo json_encode($usersList);
    } else {
        http_response_code(404);
    }
}




function getUser($link, $user_id)
{
    $admin = checkRole($link);
    if ($admin == 'user' || $admin == 'not_authorized') {
        $users = mysqli_query($link, "SELECT users.active, cities.name FROM users 
    INNER JOIN cities on cities.id = users.city
    where $user_id = users.id");
    } else if ($admin == 'admin') {
        $users = mysqli_query($link, "SELECT active, birthday, cities.name, roles.role FROM users 
        INNER JOIN cities on users.city = cities.id
        INNER JOIN roles on users.role = roles.id
        where $user_id = users.id");
    }
    if (!$users) {
        http_response_code(404);
    } else {
        $user = mysqli_fetch_assoc($users);
        echo json_encode($user);
    }
}




function postUser($link, $data)
{


    $name = $data['Name'];
    $surmane = $data['Surname'];
    $username = $data['Username'];
    $password = $data['Password'];
    if (isset($data['Birthday'])) {
        $birthday = $data['Birthday'];
    }
    $check_username = "SELECT * FROM users where 'username' = '$username'";
    $get_username = mysqli_query($link, $check_username);
    $count = mysqli_num_rows($get_username);
    if ($count >= 1) {
        $res = [
            "Error" => "Username alredy exists"
        ];
        echo json_encode($res);
        http_response_code(404);
    } else {
        if (isset($birthday)) {
            $query =  "INSERT INTO `users` (`username`, `password`, `name`, `surname`, `birthday`) VALUES ('$username', '$password', '$name', '$surmane','$birthday')";
        } else {
            $query =  "INSERT INTO `users` (`username`, `password`, `name`, `surname`, `birthday`) VALUES ('$username', '$password', '$name', '$surmane',NULL )";
        }
        $res = mysqli_query($link, $query);
        if ($res) {
            http_response_code(201);
            $user_role = checkRole($link);
            if ($user_role != 'admin') {
                loginBearer($link, $data);
            }
        } else {
            http_response_code(404);
        }
    }
}

function postUserAvatar($link, $user_id)
{
    $owner = owner($link, $user_id);
    $admin = checkRole($link);
    if ($owner == 'owner' || $admin == 'admin') {
        $msg = "";

        // If upload button is clicked ...

        $render = "SELECT * FROM photos ORDER BY id DESC LIMIT 1";
        $id = mysqli_query($link, $render);


        $filename = $user_id . $_FILES["File"]["name"];
        $tempname = $_FILES["File"]["tmp_name"];
        $folder = "image/" . $filename;

        $check_avatar = "SELECT *
    FROM photos
    WHERE photos.user_id = $user_id";

        $avatar = mysqli_query($link, $check_avatar);
        $count = mysqli_num_rows($avatar);
        if ($count == 0) {

            // Get all the submitted data from the form

            $sql = "INSERT INTO photos (link, user_id) VALUES ('$filename','$user_id')";
            if (!$sql) {
                http_response_code(404);
                exit;
            }
            // Execute query
            //echo $link->insert_id;
            $result = mysqli_query($link, $sql);

            // Now let's move the uploaded image into the folder: image
            if ($result) {
                if (move_uploaded_file($tempname, $folder)) {
                    $msg = "Image uploaded successfully";
                    $new_photo_id = $link->insert_id;
                    $update_user = "UPDATE users SET `avatar` = $new_photo_id WHERE `users`.`id` = $user_id";
                    mysqli_query($link, $update_user);
                    $res = [
                        "Success" => "Image updated",
                    ];
                    echo json_encode($res);
                } else {
                    http_response_code(404);
                    $msg = "Failed to upload image";
                }
            }
        } else {
            $get_link = "SELECT link FROM photos where user_id = $user_id";
            $glink = mysqli_query($link, $get_link);
            $delete_link = "image/" . mysqli_fetch_assoc($glink)['link'];
            unlink($delete_link);
            $update = "UPDATE `photos` SET `link` = '$filename' WHERE `photos`.`user_id` = '$user_id'";
            $up = mysqli_query($link, $update);
            if ($up) {
                move_uploaded_file($tempname, $folder);
                http_response_code(201);
                $res = [
                    "Success" => "Image updated",
                ];
                echo json_encode($res);
            } else {
                http_response_code(404);
            }
        }
    }
}

function loginBearer($link, $data)
{
    $username = $data['Username'];
    $password = $data['Password'];
    //echo "Bearer $token";
    if (isset($username) && isset($password)) {
        $find_user = "SELECT * FROM users WHERE username='$username' and password='$password'";
        $wwaw = mysqli_query($link, $find_user);
        $count = mysqli_num_rows($wwaw);
        if ($count == 0) {
            http_response_code(404);
        } else {
            $user_id = mysqli_fetch_assoc($wwaw)['id'];
            $find_user_hex = "SELECT * FROM tokens where user_id = '$user_id'";
            $hex_user = mysqli_query($link, $find_user_hex);
            $count = mysqli_num_rows($hex_user);
            if ($count >= 1) {
                $delete = "DELETE FROM `tokens` WHERE `tokens`.`user_id` = '$user_id'";
                mysqli_query($link, $delete);
            }
            $bin = bin2hex(random_bytes(32));
            $token =  "Bearer " . $bin;
            $create = "INSERT INTO `tokens` (`token`, `user_id`) VALUES ('$token', '$user_id');";
            mysqli_query($link, $create);
            http_response_code(201);
            $res = [
                "Bearer token" => "$bin"
            ];
            echo json_encode($res);
        }
    } else {
        http_response_code(400);
    }
}



function logoutBearer($link)
{
    $bearer = apache_request_headers()['Authorization'];
    //echo $bearer;
    $select = "SELECT * FROM `tokens` WHERE `tokens`.`token` = '$bearer'";
    $tokens = mysqli_query($link, $select);
    $count = mysqli_num_rows($tokens);
    if ($count > 0) {
        $delete = "DELETE FROM `tokens` WHERE `tokens`.`token` = '$bearer'";
        if (mysqli_query($link, $delete)) {
            $res = [
                "Success" => "Have a good day!"
            ];
            echo json_encode($res);
        }
    } else {
        http_response_code(404);
    }
}

function changeCity($link, $user_id, $data)
{
    $admin = checkRole($link);
    $owner = owner($link, $user_id);

    if ($owner == 'owner' || $admin == 'admin') {
        $city_id = $data['CityID'];
        $update = "UPDATE users SET city = '$city_id' WHERE id = '$user_id'";
        $res = mysqli_query($link, $update);
        if (!$res) {
            http_response_code(400);
        } else {
            http_response_code(201);
        }
    } else {
        http_response_code(418);
    }
}


function changeStatus($link, $user_id, $data)
{
    $admin = checkRole($link);
    $owner = owner($link, $user_id);

    if ($owner == 'owner' || $admin == 'admin') {

        switch ($data['StatusID']) {
            case 1:
                $status = Status::STATUS1;
                break;
            case 2:
                $status = Status::STATUS2;
                break;
            case 3:
                $status = Status::STATUS3;
                break;
            default:
                $status = Status::STATUS4;
                break;
        }
        $update = "UPDATE users SET active = '$status' WHERE id = '$user_id'";
        $res = mysqli_query($link, $update);
        if (!$res) {
            http_response_code(400);
        } else {
            http_response_code(201);
        }
    } else {
        http_response_code(418);
    }
}

function changeRole($link, $user_id, $data)
{
    $admin = checkRole($link);
    if ($admin == 'admin') {
        switch ($data['RoleID']) {
            case 1:
                $role = Role::ADMIN;
                break;
            case 2:
                $role = Role::MODERATOR;
                break;
            default:
                $role = Role::USER;
                break;
        }
        $update = "UPDATE users SET role = '$role' WHERE id = '$user_id'";
        echo $update;
        $res = mysqli_query($link, $update);
        if (!$res) {
            http_response_code(400);
        } else {
            http_response_code(201);
        }
    } else {
        http_response_code(418);
    }
}

function deleteUser($link, $user_id)
{
    $admin = checkRole($link);
    if ($admin == 'admin') {
        $delete = "DELETE FROM `users` WHERE `users`.`id` = $user_id";
        $res = mysqli_query($link, $delete);
        if (!$res) {
            http_response_code(400);
        }
    } else {
        http_response_code(418);
    }
}

function getCity($link, $city_id)
{ {
        $select = "SELECT * FROM cities WHERE id = '$city_id'";
        $res = mysqli_query($link, $select);
    }
    if (!$res) {
        http_response_code(404);
    } else {
        $city = mysqli_fetch_assoc($res);
        echo json_encode($city);
    }
}
function getAllCity($link)
{ {
        $select = "SELECT * FROM cities";
        $cities = mysqli_query($link, $select);
    }
    if ($cities) {
        $citiesList = [];
        while ($user = mysqli_fetch_assoc($cities)) {
            $citiesList[] = $user;
        }

        echo json_encode($citiesList);
    } else {
        http_response_code(404);
    }
}
function getCityWithPeople($link, $city_id)
{
    $select_city = "SELECT * FROM Cities WHERE id = '$city_id'";
    $scity = mysqli_query($link, $select_city);

    if (!$scity) {
        http_response_code(404);
    } else {
        $city = mysqli_fetch_assoc($scity);
        $id_city = $city['id'];
        $select_people = "SELECT id,username,city,role,active FROM users WHERE city = '$id_city'";
        $people = mysqli_query($link, $select_people);
        while ($user = mysqli_fetch_assoc($people)) {
            $peopleList[] = $user;
        }


        $list = array("City" => $city, "People" => $peopleList);
        echo json_encode($list);
    }
}
function postCity($link, $data)
{
    $admin = checkRole($link);
    if ($admin == 'admin') {
        $name = $data['Name'];
        $create = "INSERT INTO `cities` (`id`, `name`) VALUES (NULL, '$name')";
        $res = mysqli_query($link, $create);
    } else {
        http_response_code(418);
    }
    if (!$res) {
        http_response_code(404);
    }
}

function  changeCityName($link, $city_id, $data)
{
    $admin = checkRole($link);
    if ($admin == 'admin') {
        $name = $data['Name'];
        $update = "UPDATE `cities` SET `name` = '$name' WHERE `cities`.`id` = $city_id";
        $res = mysqli_query($link, $update);
        if (!$res) {
            http_response_code(404);
        }
    } else {
        http_response_code(418);
    }
}


function deleteCity($link, $city_id)
{
    $admin = checkRole($link);
    if ($admin == 'admin') {
        $delete = "DELETE FROM `cities` WHERE `id` = $city_id";
        $res = mysqli_query($link, $delete);
        if (!$res) {
            http_response_code(400);
        }
    } else {
        http_response_code(418);
    }
}

function userPhotos($link)
{
    $owner = owner2($link);
    if ($owner == 'owner') {
        $bearer = apache_request_headers()['Authorization'];
        $select = "SELECT user_id FROM `tokens` WHERE `tokens`.`token` = '$bearer'";
        $get_username = mysqli_query($link, $select);
        if ($get_username) {
            $user = mysqli_fetch_assoc($get_username)['user_id'];
            $select_photos = "SELECT * FROM photos WHERE 'user_id' = '$user'";
            $get_photos = mysqli_query($link, $select_photos);
            if ($get_photos) {

                $photoList = [];
                while ($photo = mysqli_fetch_assoc($get_photos)) {

                    $photoList[] = $photo;
                }
                echo json_encode($photoList);
            }
        }
    }
}
function allUserRole($link)
{
    $select = "SELECT * FROM roles";
    $roles = mysqli_query($link, $select);
    if ($roles) {

        $rolesList = [];
        while ($role = mysqli_fetch_assoc($roles)) {

            $rolesList[] = $role;
        }
        echo json_encode($rolesList);
    } else {
        http_response_code(400);
    }
}
function userRole($link, $role_id)
{
    $select = "SELECT * FROM roles WHERE id = '$role_id'";
    $roles = mysqli_query($link, $select);
    if ($roles) {
        $role = mysqli_fetch_assoc($roles);

        echo json_encode($role);
    } else {
        http_response_code(400);
    }
}
function postRole($link, $data)
{
    $admin = checkRole($link);
    if ($admin == 'admin') {
        $name = $data['Name'];
        $create = "INSERT INTO `roles` (`id`, `role`) VALUES (NULL, '$name')";
        $role = mysqli_query($link, $create);
        if (!$role) {
            http_response_code(400);
        }
        http_response_code(418);
    }
}
function  changeRoleName($link, $role_id, $data)
{
    $admin = checkRole($link);
    if ($admin == 'admin') {
        $name = $data['Name'];
        $update = "UPDATE `roles` SET `role` = '$name' WHERE `roles`.`id` = $role_id";
        $res = mysqli_query($link, $update);
        if (!$res) {
            http_response_code(404);
        }
    } else {
        http_response_code(418);
    }
}

function deleteRole($link, $role_id)
{
    $admin = checkRole($link);
    if ($admin == 'admin') {
        $delete = "DELETE FROM `roles` WHERE `id` = $role_id";
        $res = mysqli_query($link, $delete);
        if (!$res) {
            http_response_code(400);
        }
    } else {
        http_response_code(418);
    }
}

function getAllUsersPosts($link, $user_id)
{
    $select_user = "SELECT * FROM users WHERE id = '$user_id'";
    $suser = mysqli_query($link, $select_user);

    if (!$suser) {
        http_response_code(404);
    } else {
        $user = mysqli_fetch_assoc($suser);
        $select_posts = "SELECT * FROM posts WHERE user_id = $user_id";
        $posts = mysqli_query($link, $select_posts);
        while ($post = mysqli_fetch_assoc($posts)) {
            $postList[] = $post;
        }


        $list = array("User" => $user, "Posts" => $postList);
        echo json_encode($list);
    }
}

function allPosts($link)
{
    $select = "SELECT * FROM posts";
    $posts = mysqli_query($link, $select);
    if ($posts) {
        while ($post = mysqli_fetch_assoc($posts)) {
            $postList[] = $post;
        }
        echo json_encode($postList);
    }
}
function getPost($link, $post_id)
{
    $select = "SELECT * FROM posts where id = $post_id";
    $posts = mysqli_query($link, $select);
    if ($posts) {
        while ($post = mysqli_fetch_assoc($posts)) {
            $postList[] = $post;
        }
        echo json_encode($postList);
    }
}

function postPost($link, $data)
{
    $admin = checkRole($link);
    if ($admin == 'admin') {
        $text = $data['Text'];
        $user_id = $data['UserID'];
        $date = date('Y-m-d H:i:s');
        $create = "INSERT INTO `posts` (`id`, `user_id`, `text`, `date`) VALUES (NULL, '$user_id', '$text', '$date')";
        $post = mysqli_query($link, $create);
        if (!$post) {
            http_response_code(400);
        }
        http_response_code(418);
    }
}
function changePostText($link, $post_id, $data)
{
    $admin = checkRole($link);
    if ($admin == 'admin') {
        $text = $data['Text'];
        $date = date('Y-m-d H:i:s');
        $update = "UPDATE `posts` SET `text` = '$text', `date` = '$date' WHERE `posts`.`id` = $post_id";
        $res = mysqli_query($link, $update);
        if (!$res) {
            http_response_code(404);
        }
    } else {
        http_response_code(418);
    }
}

function deletePost($link, $post_id)
{
    $admin = checkRole($link);
    if ($admin == 'admin') {
        $delete = "DELETE FROM `posts` WHERE `id` = $post_id";
        $res = mysqli_query($link, $delete);
        if (!$res) {
            http_response_code(400);
        }
    } else {
        http_response_code(418);
    }
}

function getMessage($link, $mess_id)
{
    $message_select = "SELECT * FROM messages WHERE `id`=$mess_id";
    $res = mysqli_query($link, $message_select);
    if (!$res) {
        http_response_code(400);
    } else {
        $message = mysqli_fetch_assoc($res);
        $perm_owner = $message['user_id'];
        $perm_rec = $message['rec_id'];
        $bearer = apache_request_headers()['Authorization'];
        $select = "SELECT user_id FROM `tokens` WHERE `tokens`.`token` = '$bearer'";
        $get_username = mysqli_query($link, $select);
        if ($get_username) {
            $user = mysqli_fetch_assoc($get_username)['user_id'];
            if ($user == $perm_owner || $user == $perm_rec) {
                echo json_encode($message);
            }
        }
    }
}

function getAllMessage($link)
{

    $bearer = apache_request_headers()['Authorization'];
    $select = "SELECT user_id FROM `tokens` WHERE `tokens`.`token` = '$bearer'";
    $get_username = mysqli_query($link, $select);
    if ($get_username) {
        $user = mysqli_fetch_assoc($get_username)['user_id'];

        $select_inc = "SELECT * FROM `messages` WHERE user_id = $user";
        $inc = mysqli_query($link, $select_inc);
        $select_rec = "SELECT * FROM `messages` WHERE rec_id = $user";
        $rec = mysqli_query($link, $select_rec);
        $messagesList = [];
        if ($inc) {
            while ($mes = mysqli_fetch_assoc($inc)) {
                $messagesList[] = $mes;
            }
        }
        if ($rec) {
            while ($mes = mysqli_fetch_assoc($rec)) {
                $messagesList[] = $mes;
            }
        }
        echo json_encode($messagesList);
    }
}

function postMessage($link, $rec_id, $data)
{

    $bearer = apache_request_headers()['Authorization'];
    $select = "SELECT user_id FROM `tokens` WHERE `tokens`.`token` = '$bearer'";
    $get_username = mysqli_query($link, $select);
    if ($get_username) {
        $text = $data['Text'];
        $date = date('Y-m-d H:i:s');
        $user = mysqli_fetch_assoc($get_username)['user_id'];
        $create = "INSERT INTO `messages` (`id`, `user_id`, `rec_id`, `text`, `date`) VALUES (NULL, '$user', '$rec_id', '$text', '$date');";
        $res = mysqli_query($link, $create);
        if (!$res) {
            http_response_code(404);
        }
    }
}

function deleteMessage($link, $mess_id)
{
    $message_select = "SELECT * FROM messages WHERE `id`=$mess_id";
    $admin = checkRole($link);
    $res = mysqli_query($link, $message_select);
    if (!$res) {
        http_response_code(400);
    } else {
        $message = mysqli_fetch_assoc($res);
        $perm_owner = $message['user_id'];
        $perm_rec = $message['rec_id'];
        $bearer = apache_request_headers()['Authorization'];
        $select = "SELECT user_id FROM `tokens` WHERE `tokens`.`token` = '$bearer'";
        $get_username = mysqli_query($link, $select);
        if ($get_username) {
            $user = mysqli_fetch_assoc($get_username)['user_id'];
            if ($user == $perm_owner || $user == $perm_rec || $admin == 'admin') {
                $delete = "DELETE FROM `messages` WHERE `id` = $mess_id";
                $res = mysqli_query($link, $delete);
                if (!$res) {
                    http_response_code(400);
                }
            } else {
                http_response_code(418);
            }
        }
    }
}
