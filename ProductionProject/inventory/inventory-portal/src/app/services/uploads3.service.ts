import { Injectable } from '@angular/core';
import * as S3 from 'aws-sdk/clients/s3';

@Injectable({
  providedIn: 'root',
})
export class UploadS3Service {
  constructor() {}
  uploadFile(file: any, filePath: any) {
      debugger;
    return new Promise((resolve, reject) => {
      const contentType = file.type;
      const bucket = new S3({
        accessKeyId: 'AKIAQDRSIWHCSQVQ4KVX',// ACCESS_KEY_ID
        secretAccessKey: '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',// SECRET_ACCESS_KEY
        region: 'us-west-2',// BUCKET_REGION
      });
      const params = {
        Bucket: 'systemsource',//BUCKET_NAME
        Key: filePath,
        Body: file,
        ACL: 'public-read',
        ContentType: contentType,
      };
      bucket.upload(params, function (err: any, data: any) {
        if (err) {
          reject(err);
        }
        resolve(data);
      });
    });
  }
}