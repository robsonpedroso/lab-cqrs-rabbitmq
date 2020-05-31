Vagrant.configure("2") do |config|

  config.vm.box = "ubuntu/trusty64"

  config.vm.define "rabbitmq" do |rabbitmq|
    rabbitmq.vm.network "private_network", ip: "192.168.33.10"

    rabbitmq.vm.provider "virtualbox" do |vb|
     vb.memory = 1024
     vb.cpus = 2
     vb.name = "rabbitmq-cqrs"
    end

    rabbitmq.vm.provision "shell", inline: <<-SHELL
      apt-get update -y
      echo 'deb http://www.rabbitmq.com/debian/ testing main' | sudo tee /etc/apt/sources.list.d/rabbitmq.list
      wget -O- https://www.rabbitmq.com/rabbitmq-release-signing-key.asc | sudo apt-key add -
      apt-get update -y
      apt-get install -y rabbitmq-server
      rabbitmq-plugins enable rabbitmq_management
      rabbitmqctl add_user admin 123
      rabbitmqctl set_user_tags admin administrator
      rabbitmqctl set_permissions -p / admin ".*" ".*" ".*"
    SHELL
  end
end
