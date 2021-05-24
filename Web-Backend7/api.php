<?php


//$bear = apache_request_headers();

require('service/functions.php');
header('Content-type: application/json');
require('DBconnect.php');
//$_SERVER['PHP_AUTH_USER'] = 'vova';

$method = $_SERVER['REQUEST_METHOD'];
$data = getFormData($method);

$q = $_GET['q'];
$params = explode('/', $q);

switch ($method) {
    case 'GET':
        switch ($params[1]) {
            case 'users':
                if (isset($params[2]) && isset($params[3]) && $params[3] == 'posts') {
                    getAllUsersPosts($link, $params[2]);
                } else if (isset($params[2])) {
                    getUser($link, $params[2]);
                } else {
                    getAllUsers($link);
                }
                break;
            case 'city':
                if (isset($params[2]) && isset($params[3]) && $params[3] == 'people') {
                    getCityWithPeople($link, $params[2]);
                } else if (isset($params[2])) {
                    getCity($link, $params[2]);
                } else {
                    getAllCity($link);
                }
                break;
            case 'photos':
                userPhotos($link);
                break;
            case 'role':
                if (isset($params[2])) {
                    userRole($link, $params[2]);
                } else {
                    allUserRole($link);
                }
                break;
            case 'posts':
                if (isset($params[2])) {
                    getPost($link, $params[2]);
                } else {
                    allPosts($link);
                }
                break;
            case 'messages':
                if (isset($params[2])) {
                    getMessage($link, $params[2]);
                } else {
                    getAllMessage($link);
                }
                break;
        }
        break;
    case 'POST':
        switch ($params[1]) {
            case 'users':
                if (isset($params[2]) && isset($params[3]) && $params[3] == 'avatar') {
                    postUserAvatar($link, $params[2]);
                } else {
                    postUser($link, $data);
                }
                break;
            case 'login':
                loginBearer($link, $data);
                break;

            case 'logout':
                logoutBearer($link);
                break;
            case 'city':
                postCity($link, $data);
                break;
            case 'role':
                postRole($link, $data);
                break;
            case 'posts':
                postPost($link, $data);
                break;
            case 'messages':
                if (isset($params[2])) {
                    postMessage($link, $params[2], $data);
                }
                break;
        }
        break;
    case 'PATCH':
        switch ($params[1]) {
            case 'users':
                if (isset($params[2]) && isset($params[3]) && $params[3] == 'city') {
                    changeCity($link, $params[2], $data);
                } else if (isset($params[2]) && isset($params[3]) && $params[3] == 'status') {
                    changeStatus($link, $params[2], $data);
                } else if (isset($params[2]) && isset($params[3]) && $params[3] == 'role') {
                    changeRole($link, $params[2], $data);
                }
                break;
            case 'city':
                if (isset($params[2])) {
                    changeCityName($link, $params[2], $data);
                }
                break;
            case 'role':
                if (isset($params[2])) {
                    changeRoleName($link, $params[2], $data);
                }
                break;
            case 'posts':
                if (isset($params[2])) {
                    changePostText($link, $params[2], $data);
                }
                break;
        }
        break;
    case 'DELETE':
        switch ($params[1]) {
            case 'users':
                if (isset($params[2])) {
                    deleteUser($link, $params[2]);
                }
                break;
            case 'city':
                if (isset($params[2])) {
                    deleteCity($link, $params[2]);
                }
            case 'role':
                if (isset($params[2])) {
                    deleteRole($link, $params[2]);
                }
                break;
            case 'posts':
                if (isset($params[2])) {
                    deletePost($link, $params[2]);
                }
                break;
            case 'messages':
                if (isset($params[2])) {
                    deleteMessage($link, $params[2]);
                }
        }
        break;
}


//echo $type;