<?php
$link = mysqli_connect("localhost", "root", "root","backend7");

if (!$link) {
    echo "Ошибка: Невозможно установить соединение с MySQL." . PHP_EOL;
    echo "Код ошибки errno: " . mysqli_connect_errno() . PHP_EOL;
    echo "Текст ошибки error: " . mysqli_connect_error() . PHP_EOL;
    exit;
}

echo "Соединение с MySQL установлено!" . PHP_EOL;
echo "Информация о сервере: " . mysqli_get_host_info($link) . PHP_EOL;


$query = "INSERT INTO role (role) 
VALUES ('admin'), ('moderator'), ('user')";
$result = mysqli_query($link, $query);


// $res = $mysqli->query("SELECT id FROM test ORDER BY id ASC");
// if (!$res) //SQL
// {
//     echo "Не удалось выполнить запрос: (" . $mysqli->errno . ") " . $mysqli->error;
// }
// else
// {
//     while ($row = $res->fetch_assoc()) 
//     {
//         echo " id = " . $row['id'] . "\n";
//     }
// }


//mysqli_close($link); //закрытие соединения, выполняется, когда мы закончили работать с БД
?>
