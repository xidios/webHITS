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
require('add_link.php');

$cities_quary = "SELECT * FROM cities";
$cities = mysqli_query($link, $cities_quary);

if (isset($_POST['username']) && isset($_POST['password'])) {
    $user = new User(
        $_POST['username'],
        $_POST['password'],
        $_POST['name'],
        $_POST['surname'],
        $_POST['birthday'],
        $_POST['city']
    );


    $query = "INSERT INTO users (username, password, name, surname, birthday, city) 
    VALUES (
    '$user->username',
    '$user->password',
    '$user->name',
    '$user->surname',
    '$user->birthday',
    '$user->city')";

    $result = mysqli_query($link, $query);

    if ($result) {
        $smsg = "Повезло, повезло";
        if ($_FILES["photo"]["name"] != "") {
            $new_user_id = $link->insert_id;
            if (add_photo($link, $new_user_id)) {
                $new_photo_id = $link->insert_id;
                $update_user = "UPDATE users SET `avatar` = $new_photo_id WHERE `users`.`id` = $new_user_id";
                mysqli_query($link, $update_user);
            }
        }
    } else {
        $fsmsg = "Не повезло, не повезло";
    }
}

?>

<body>
    <div class="container">
        <form class="form-singin" method="POST" enctype="multipart/form-data">
            <?php if (!$_SESSION['username']) { ?>
                <h2>Registration</h2>
                <?php if (isset($smsg)) { ?>
                    <div class="alert alert-success" role="alert"> <?php echo $smsg; ?> </div><?php } ?>
                <?php if (isset($fsmsg)) { ?>
                    <div class="alert alert-danger" role="alert"> <?php echo $fsmsg; ?> </div><?php } ?>

                <input type="text" name="username" class="form-control" placeholder="Username" required>
                <input type="password" name="password" class="form-control" placeholder="Password" required>
                <input type="text" name="name" class="form-control" placeholder="Name" required>
                <input type="text" name="surname" class="form-control" placeholder="Surmane" required>
                <input type="date" name="birthday" class="form-control" placeholder="Birthday" required>
                <select type="text" name="city" class="form-control" placeholder="City" required>
                    <?php while ($city = $cities->fetch_assoc()) {
                        echo '<option value="' . $city['id'] . '">' . $city['name'] . '</option>';
                    } ?>

                </select>
                <input type="file" name="photo" class="form-control">
                <button class="btn btn-lg btn-primary btn-block" type="submit">Registration</button>
                <a href="login.php" class="btn btn-lg btn-submit btn block">Login</a>
            <?php } else { ?>
                <h2 align="center">Hello <?php echo $_SESSION['username'] ?></h2>
                <a href='logout.php' class='btn btn-lg btn-submit btn-block'>Logout</a>
            <?php } ?>
        </form>
    </div>

</body>

</html>