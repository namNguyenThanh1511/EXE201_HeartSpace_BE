docker build -t namnt1511/heartspace-api:latest -f Dockerfile .
docker push namnt1511/heartspace-api:latest
ssh root@103.126.161.41
cd /root/heartspace/
docker-compose -f docker-compose.prod.yml pull
docker-compose -f docker-compose.prod.yml up -d
