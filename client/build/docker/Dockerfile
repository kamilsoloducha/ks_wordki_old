FROM node:14 as build

# Disable apt-get warnings
# https://github.com/wv-gis/mudak-wrm-public/issues/2
ENV APT_KEY_DONT_WARN_ON_DANGEROUS_USAGE=DontWarn

# Resolve apt-utils warning
# https://github.com/phusion/baseimage-docker/issues/319#issuecomment-245857919
# https://stackoverflow.com/questions/51023312/docker-having-issues-installing-apt-utils
ARG DEBIAN_FRONTEND=noninteractive
RUN apt-get update && apt-get install -yq --no-install-recommends apt-utils


# Install chrome for tests
RUN wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | apt-key add - &&\
	sh -c 'echo "deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main" >> /etc/apt/sources.list.d/google.list' &&\
	apt-get update &&\
  apt-get install -yq google-chrome-stable


RUN mkdir -p /usr/src/app && chown node:node /usr/src/app
WORKDIR /usr/src/app

ENV PATH /usr/src/app/node_modules/.bin:$PATH

# Install packages with npm
#COPY package.json package-lock.json ./
#RUN npm install

# Install packages with yarn
COPY package.json yarn.lock ./
RUN yarn install

COPY . .

#####################################################
# Build app for tests e2e
#####################################################
FROM build as pre-e2e

RUN npm run build:e2e

#####################################################
# Run app for tests e2e
#####################################################

FROM nginx:alpine as e2e

RUN rm -rf /usr/share/nginx/html/*
COPY --from=pre-e2e /usr/src/app/dist/wordki /usr/share/nginx/html
COPY ./nginx.conf /etc/nginx/nginx.conf

CMD sed -i -e 's/$PORT/'81'/g' /etc/nginx/nginx.conf && nginx -g 'daemon off;'


#####################################################
# Build app for release
#####################################################

FROM build as pre-release

RUN npm run build:prod


#####################################################
# Run app for release
#####################################################

FROM nginx:alpine as release

RUN rm -rf /usr/share/nginx/html/*
COPY --from=pre-release /usr/src/app/dist/wordki /usr/share/nginx/html
COPY ./nginx.conf /etc/nginx/nginx.conf

CMD sed -i -e 's/$PORT/'"$PORT"'/g' /etc/nginx/nginx.conf && nginx -g 'daemon off;'