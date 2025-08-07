'use client'

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import styles from "@/styles/recipts-page/hero/hero.module.css"
import { getInactiveResources } from '@/services/api/resourceClient';

interface Resource{
    id:number;
    name:string;
}


export default function Hero() {
  const router = useRouter();
  const [archiveResources, setArchiveResources] = useState<Resource[]>([]);

  useEffect(() => {
    async function fetchData() {
      try {
        const data = await getInactiveResources();
        setArchiveResources(data);
      } catch (error) {
        console.error(error);
      }
    }

    fetchData();
  }, []);
  return (
    <main className={styles.mainContainer}>
        <h1 className={styles.headerText}>Ресурсы</h1>
        <div className={styles.buttonGroup}>
          <button className={styles.acceptButton} onClick={() => router.push('/resources-page/')}>К рабочим</button>
        </div>
        <table className={styles.table}>
          <thead>
            <tr>
              <th>Наименование</th>
            </tr>
          </thead>
          <tbody>
            {archiveResources.map(resource => (
              <tr key={resource.id} onClick={() => router.push(`/edit-archiveresource/${resource.id}`)}>
                <td>{resource.name}</td>
              </tr>
            ))}
          </tbody>
        </table>
    </main>
  );
}
