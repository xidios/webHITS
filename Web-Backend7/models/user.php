<?php
class User
{
    public $username;
    public $password;
    public $name;
    public $surname;
    public $birthday;
    public $city;

    public function __construct($username, $password, $name, $surname, $birthday, $city)
    {
        $this->username = $username;
        $this->password = $password;
        $this->name = $name;
        $this->surname = $surname;
        $this->birthday = $birthday;
        $this->city = $city;
    }
    //public $Avatar;
    //public $Status;
}
