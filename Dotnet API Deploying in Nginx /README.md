This README provides a step-by-step guide for setting up a Jenkins server on AWS EC2, deploying a .NET application, and configuring Nginx for reverse proxy.

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Creating EC2 Instances](#creating-ec2-instances)
3. [Setting Up Jenkins Server](#setting-up-jenkins-server)
4. [Installing .NET SDK](#installing-net-sdk)
5. [Configuring Nginx](#configuring-nginx)
6. [Granting Jenkins Permissions](#granting-jenkins-permissions)
7. [Creating a .NET Pipeline in Jenkins](#creating-a-net-pipeline-in-jenkins)
8. [Conclusion](#conclusion)

## Prerequisites

- AWS account
- Basic knowledge of EC2 and Jenkins
- Git installed on the Jenkins server

## Step 1: Creating EC2 Instances

1. **Log in to AWS Management Console**.
2. **Navigate to EC2 Dashboard**.
3. **Launch two instances**:
   - **Jenkins Server**: Choose an appropriate Amazon Machine Image (AMI) and instance type (e.g., Ubuntu 20.04).
   - **Deployment Server**: Choose the same or similar AMI and instance type.
4. **Configure Security Groups**:
   - Allow HTTP (port 80) and SSH (port 22) access.
   - Ensure Jenkins server can communicate with the deployment server.

## Step 2: Setting Up Jenkins Server

1. **Connect to your Jenkins server via SSH**.
2. **Install Java** (required for Jenkins):
   ```bash
   sudo apt update
   sudo apt install openjdk-11-jdk -y
   ```
3. **Add Jenkins repository and key**:
   ```bash
   wget -q -O - https://pkg.jenkins.io/debian/jenkins.io.key | sudo apt-key add -
   sudo sh -c 'echo deb http://pkg.jenkins.io/debian-stable binary/ > /etc/apt/sources.list.d/jenkins.list'
   sudo apt update
   ```
4. **Install Jenkins**:
   ```bash
   sudo apt install jenkins -y
   ```
5. **Start Jenkins**:
   ```bash
   sudo systemctl start jenkins
   sudo systemctl enable jenkins
   ```

## Step 3: Installing .NET SDK

1. **Connect to your deployment server via SSH**.
2. **Add the Microsoft package signing key**:
   ```bash
   wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb
   sudo dpkg -i packages-microsoft-prod.deb
   ```
3. **Install the SDK**:
   ```bash
   sudo apt-get update
   sudo apt-get install dotnet-sdk-6.0 -y
   ```
4. **Verify installation**:
   ```bash
   dotnet --version
   ```

## Step 4: Configuring Nginx

1. **Install Nginx**:
   ```bash
   sudo apt-get install nginx -y
   ```
2. **Configure Nginx**:
   Edit the Nginx configuration file (`/etc/nginx/sites-available/default`) to include the following configuration:
   ```nginx
   http {
       map $http_connection $connection_upgrade {
           "~*Upgrade" $http_connection;
           default keep-alive;
       }

       server {
           listen 80;
           server_name _;
           location / {
               proxy_pass http://localhost:5000;
               proxy_http_version 1.1;
               proxy_set_header Upgrade $http_upgrade;
               proxy_set_header Connection $connection_upgrade;
               proxy_set_header Host $host;
               proxy_cache_bypass $http_upgrade;
               proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
               proxy_set_header X-Forwarded-Proto $scheme;
           }
       }
   }
   ```
3. **Restart Nginx**:
   ```bash
   sudo systemctl restart nginx
   ```

## Step 5: Granting Jenkins Permissions

1. **Edit the sudoers file**:
   ```bash
   sudo visudo
   ```
2. **Add the following line at the end**:
   ```bash
   jenkins ALL=(ALL) NOPASSWD: ALL
   ```

## Step 6: Creating a .NET Pipeline in Jenkins

1. **Open Jenkins in a web browser** (typically at `http://your-jenkins-server:8080`).
2. **Create a new pipeline job**.
3. **Add the following pipeline script**:

```groovy
pipeline {
    agent {
        label 'dotnet'
    }
    environment {
        REPO_URL = 'https://github.com/Aamantamboli/Dotnetapi.git'
        CLONE_DIR = "/var/lib/jenkins/workspace/${JOB_NAME}"
        PROJECT_DIR = "${CLONE_DIR}/KubernetesAutoClusterAPI"
        PUBLISH_DIR = "${PROJECT_DIR}/bin/Release/net6.0/publish"
        SERVICE_FILE = '/etc/systemd/system/kubernetesautoclusterapi.service'
    }
    
    stages {
        stage('Clone Repository') {
            steps {
                script {
                    if (fileExists(PROJECT_DIR)) {
                        dir(PROJECT_DIR) {
                            sh 'git pull origin main'
                        }
                    } else {
                        sh "git clone ${REPO_URL} ${CLONE_DIR}"
                    }
                }
            }
        }

        stage('Build and Publish') {
            steps {
                dir(PROJECT_DIR) {
                    sh 'dotnet restore'
                    sh 'dotnet build -c Release'
                    sh "dotnet publish -c Release -o ${PUBLISH_DIR}"
                }
            }
        }

        stage('Deploy') {
            steps {
                script {
                    sh "sudo mkdir -p /wwwroot/"
                    sh "sudo cp -r ${PUBLISH_DIR}/* /wwwroot/"
                    sh """
                    echo '[Unit]
                    Description=Kubernetes Auto Cluster API

                    [Service]
                    WorkingDirectory=${PUBLISH_DIR}
                    ExecStart=/usr/bin/dotnet ${PUBLISH_DIR}/KubernetesAutoClusterAPI.dll --urls http://localhost:5000
                    Restart=always
                    RestartSec=10
                    SyslogIdentifier=kubernetesautoclusterapi
                    User=www-data
                    Environment=ASPNETCORE_ENVIRONMENT=Production

                    [Install]
                    WantedBy=multi-user.target' | sudo tee ${SERVICE_FILE}
                    """
                    sh "sudo systemctl daemon-reload"
                    sh "sudo systemctl enable kubernetesautoclusterapi.service"
                    sh "sudo systemctl start kubernetesautoclusterapi.service"
                }
            }
        }

        stage('Check Service Status') {
            steps {
                sh 'sudo systemctl status kubernetesautoclusterapi.service'
            }
        }
    }

    post {
        always {
            echo 'Cleaning up...'
            cleanWs()
        }
    }
}
```

## Conclusion

You have successfully set up a Jenkins server on AWS EC2, installed .NET SDK, configured Nginx, and created a Jenkins pipeline to build and deploy a .NET application. You can now manage your builds and deployments through Jenkins.

For further information or troubleshooting, please refer to the official documentation for Jenkins, Nginx, and .NET SDK.
