<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, user-scalable=no, initial-scale=1.0, minimum-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie-edge">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" integrity="sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm" crossorigin="anonymous">
    <link rel="stylesheet" href="style.css">
    <title>Document</title>
</head>

<?php
session_start();
require('DBconnect.php');
require('models/user.php');

if ($_POST['action'] == 'login') {
    if (isset($_POST['username']) && isset($_POST['password'])) {
        $username = $_POST['username'];
        $password = $_POST['password'];

        $query = "SELECT username, password FROM users WHERE username='$username' and password='$password'";
        $result = mysqli_query($link, $query) or die(mysqli_error($link));
        $count = mysqli_num_rows($result);
        if ($count == 1) {
            $_SESSION['username'] = $username;
        } else {
            $fsmsg = "Ошибка проверьте логин или пароль";
        }
    }
} else if ($_POST['action'] == 'post') {
    if (isset($_POST['text'])) {
        echo "post";
        $text = $_POST['text'];
        $date = date('Y-m-d H:i:s');
        $username = $_SESSION['username'];
        $query = "SELECT * FROM users WHERE username='$username'";
        $result = mysqli_query($link, $query) or die(mysqli_error($link));
        $count = mysqli_num_rows($result);

        if ($count == 1) {
            $row = $result->fetch_assoc()['id'];
            $query_add_post = "INSERT INTO posts (user_id, text, date) 
            VALUES (
            '$row',
            '$text',
            '$date'
            )";
            $post_result = mysqli_query($link, $query_add_post);
            if (!$post_result) {
                echo "Ошибка CREATE POST";
            }
        } else {
            echo "Ошибка CREATE POST id NOT FOUND";
        }
    }
} else if ($_POST['action'] == 'message') {
    if (isset($_POST['text'])) {
        echo "message";
        $text = $_POST['text'];
        $date = date('Y-m-d H:i:s');
        $username = $_SESSION['username'];
        $query = "SELECT * FROM users WHERE username='$username'";
        $result = mysqli_query($link, $query) or die(mysqli_error($link));
        $count = mysqli_num_rows($result);

        if ($count == 1) {
            $row = $result->fetch_assoc()['id'];
            $query_add_message = "INSERT INTO messages (user_id, text, date) 
            VALUES (
            '$row',
            '$text',
            '$date'
            )";
            $massage_result = mysqli_query($link, $query_add_message);
            if (!$massage_result) {
                echo "Ошибка CREATE MESSAGE";
            }
        } else {
            echo "Ошибка CREATE MESSAGE id NOT FOUND";
        }
    }
}
if(isset($_SESSION['username'])){
    $username = $_SESSION['username'];
    require('add_link.php');
    $query = "SELECT * FROM users WHERE username='$username'";
    $result = mysqli_query($link, $query) or die(mysqli_error($link));
    $user_id = $result->fetch_assoc()['id'];
    $image = load_photo($link,$user_id);
}

// if (isset($_SESSION['username'])) {
//     $username = $_SESSION['username'];
//     echo "Hello" . $username . "";
//     echo "Вы вошли";    
// }
?>

<body>
    <div class="container">
        <form class="form-singin" method="POST">

            <?php if (!$_SESSION['username']) { ?>
                <h2>Login</h2>
                <?php if (isset($fsmsg)) { ?>
                    <div class="alert alert-danger" role="alert"> <?php echo $fsmsg; ?> </div><?php } ?>
                <input type="text" name="username" class="form-control" placeholder="Username" required>
                <input type="password" name="password" class="form-control" placeholder="Password" required>
                <input type="hidden" name="action" value="login">
                <button class="btn btn-lg btn-primary btn-block" type="submit">Login</button>
                <a href="index.php" class="btn btn-lg btn-submit btn-block">Registration</a>
            <?php } else { ?>
                <h2 align="center">Hello <?php echo $_SESSION['username'] ?></h2>
                <?php if(isset($image)) {?>
                <img src="<?php echo $image; ?>" alt="" /><?php } ?>
                <a href='logout.php' class='btn btn-lg btn-submit btn-block'>Logout</a>
            <?php } ?>
        </form>
        <?php if ($_SESSION['username']) { ?>
            <h2>Create Post</h2>
            <form class="form-singin" method="POST">
                <input type="text" name="text" class="form-control" placeholder="Text" required>
                <input type="hidden" name="action" value="post">
                <button class="btn btn-lg btn-primary btn-block" type="submit">Create post</button>
            </form>
            <h2>Create Message</h2>
            <form class="form-singin" method="POST">
                <input type="text" name="text" class="form-control" placeholder="Text" required>
                <input type="hidden" name="action" value="message">
                <button class="btn btn-lg btn-primary btn-block" type="submit">Create message</button>
            </form>
        <?php } ?>
    </div>


</body>

</html>